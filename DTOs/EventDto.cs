namespace WawAPI.DTOs;

public class EventDto
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Link { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Image { get; set; } = default!;
    public LocationDto Location { get; set; } = default!;
    public string Guid { get; set; } = default!;
    public int LikeCount { get; set; } = default!;
    public bool IsCurrent { get; set; } = default!;
    public List<string> Types { get; set; } = default!;
}
