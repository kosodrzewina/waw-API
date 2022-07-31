using System.ComponentModel.DataAnnotations.Schema;

namespace WawAPI.Models;

public class Event
{
    public int Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Link { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Guid { get; set; } = default!;
    public bool IsCurrent { get; set; } = default!;
    public ICollection<EventType> Types { get; set; } = default!;

    [NotMapped]
    public List<EventTypeEnum> TypeEnums { get; set; } = default!;

    public override string ToString()
    {
        return $"Title: {Title}\nDescription: {Description}\nLink: {Link}\nGuid: {Guid}";
    }
}
