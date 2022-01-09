namespace WawAPI.Models
{
    public class Event
    {
        public string Title { get; set; } = "Title";
        public string Description { get; set; } = "Description";
        public string Link { get; set; } = "Link";
        public string Guid { get; set; } = "Guid";

        public override string ToString()
        {
            return $"Title: {Title}\nDescription: {Description}\nLink: {Link}\nGuid: {Guid}";
        }
    }
}
