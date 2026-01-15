using Core.Model;

namespace Model.ProjectEntities;

public class LocalizationLanguageDetail : IEntity, IProjectEntity
{
    public Guid LocalizationId { get; set; }
    public int LanguageId { get; set; }
    public string? Value { get; set; }

    public virtual Localization? Localization { get; set; }
    public virtual Language? Language { get; set; }
}
