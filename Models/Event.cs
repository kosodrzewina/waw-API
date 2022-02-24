namespace WawAPI.Models;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    public string Guid { get; set; }
    public int IdEventType { get; set; }
    public EventType Type { get; set; }

    public override string ToString()
    {
        return $"Title: {Title}\nDescription: {Description}\nLink: {Link}\nGuid: {Guid}";
    }
}
