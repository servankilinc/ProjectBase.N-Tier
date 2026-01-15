using Core.Model;

namespace Model.ProjectEntities;

public class Localization : IEntity, IProjectEntity
{
    public Guid Id { get; set; }
    public string? EntityId { get; set; }
    public string? TableName { get; set; }
    public string Key { get; set; } = null!;

    public virtual ICollection<LocalizationLanguageDetail>? LocalizationLanguageDetails { get; set; }
}