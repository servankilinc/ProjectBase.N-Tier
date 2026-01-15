using Core.Enums;
using Core.Model;
using Core.Utils;
using Core.Utils.Caching;
using Core.Utils.Localization;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Model.ProjectEntities;
using Newtonsoft.Json;

namespace DataAccess.Interceptors.Helpers;

public class LocalizationHelper
{

    private readonly IServiceProvider _serviceProvider;
    private readonly ICacheService _cacheService;
    public LocalizationHelper(IServiceProvider serviceProvider, ICacheService cacheService)
    {
        _serviceProvider = serviceProvider;
        _cacheService = cacheService;
    }

    public string GenerateLocalizationKey(string? tableName, string? propertyName, string? entityId)
    {
        return $"{tableName ?? "undefined"}_{propertyName ?? "undefined"}_{entityId ?? "undefined"}";
    }
    public string GenerateLocalizationKey(ILocalizableEntity entity, LocalizablePropAttribute attr)
    {
        var entitId =
            entity is ILocalizableEntity<Guid> myGuidEntity ? myGuidEntity.Id.ToString() :
            entity is ILocalizableEntity<string> myStringEntity ? myStringEntity.Id :
            entity is ILocalizableEntity<int> myIntEntity ? myIntEntity.Id.ToString() : string.Empty;

        return $"{attr.Key}_{entitId}";
    }

    public string? ResolveLocalizationValue(string key, Languages language)
    {
        string culture = language.GetDescription();
        int languageId = (int)language;
        string cacheKey = $"localization-{key}-{languageId}";
        string[] cacheGroups = ["localization-group"];

        // 1) Try Read Cache
        CacheResponse cacheResponse = _cacheService.GetFromCache(cacheKey);
        if (cacheResponse.IsSuccess && !string.IsNullOrWhiteSpace(cacheResponse.Source))
        {
            string? cachedData = JsonConvert.DeserializeObject<string>(cacheResponse.Source);
            if (!string.IsNullOrWhiteSpace(cacheResponse.Source)) return cachedData;
        }

        // 2) Try Read Resource
        //string? resData = Core.Utils.Localization.Resources.Localization.ResourceManager.GetString(key, new CultureInfo(culture));
        string? resData = Core.Utils.Localization.Resources.Localization.ResourceManager.GetString(key); // default culture
        if (!string.IsNullOrWhiteSpace(resData))
        {
            _cacheService.AddToCache<string>(cacheKey, cacheGroups, resData);
            return resData;
        }

        // 3) Try Read Database
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (context == null) return null;
        var localization = context.Set<Localization>().Include(i => i.LocalizationLanguageDetails).FirstOrDefault(f => f.Key == key);

        if (localization == null || localization.LocalizationLanguageDetails == null) return null;

        var all = localization.LocalizationLanguageDetails.AsEnumerable();
        var langDetail =
            all.FirstOrDefault(x => x.LanguageId == languageId) ??
            all.FirstOrDefault(x => x.LanguageId == (byte)Languages.Turkish) ??
            all.FirstOrDefault();

        if (langDetail != null && !string.IsNullOrWhiteSpace(langDetail.Value))
            _cacheService.AddToCache<string>(cacheKey, cacheGroups, langDetail.Value);

        return langDetail?.Value;
    }
}