namespace WawAPI.Models;

public class Location
{
    public int IdEvent { get; set; } = default!;
    public double Longitude { get; set; } = default!;
    public double Latitude { get; set; } = default!;
    public Event Event { get; set; } = default!;
}
