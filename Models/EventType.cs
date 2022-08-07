namespace WawAPI.Models;

public class EventType
{
    public int Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public ICollection<Event> Events { get; set; } = default!;
}
