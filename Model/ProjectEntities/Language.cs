using Core.Model;

namespace Model.ProjectEntities;

public class Language : IEntity, IProjectEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public int Priority { get; set; }
    public string? ResourceFileName { get; set; }
    public int? ResourceFileVersion { get; set; }

    public virtual ICollection<LocalizationLanguageDetail>? LocalizationLanguageDetails { get; set; }
}