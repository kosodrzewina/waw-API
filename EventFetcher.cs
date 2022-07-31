using System.Diagnostics;
using System.Xml.Linq;
using WawAPI.Models;

namespace WawAPI;

public class EventFetcher
{
    private readonly EventTypeEnum[] _eventTypes;

    public EventFetcher(params EventTypeEnum[] eventTypes)
    {
        _eventTypes = eventTypes.Where(e => IsUrlValid(e.Address)).ToArray();
    }

    public List<Event> LastFetched { get; private set; } = new();

    public static bool IsUrlValid(string address)
    {
        return Uri.TryCreate(address, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    public async Task Fetch()
    {
        var events = new List<Event>();
        using var httpClient = new HttpClient();

        foreach (var eventType in _eventTypes)
        {
            Debug.WriteLine($"Fetching {eventType.Name} events...");

            var httpResponseMessage = await httpClient.GetAsync(eventType.Address);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                // TODO: do something
                continue;
            }

            var feed = XDocument.Load(eventType.Address);

            feed.Descendants()
                .Where(item => item.Name == "item")
                .ToList()
                .ForEach(
                    item =>
                    {
                        var title = item.Element("title");
                        var description = item.Element("description");
                        var link = item.Element("link");
                        var guid = item.Element("guid");

                        if (!events.Any(e => e.Guid == guid?.Value))
                        {
                            events.Add(
                                new Event
                                {
                                    // TODO: error messages should not be the params values
                                    Title =
                                        title != null && title.Value != null
                                            ? title.Value
                                            : "An error occurred when fetching title",
                                    Description =
                                        description != null && description.Value != null
                                            ? description.Value
                                            : "An error occurred when fetching description",
                                    Link =
                                        link != null && link.Value != null
                                            ? link.Value
                                            : "An error occurred when fetching link",
                                    Guid =
                                        guid != null && guid.Value != null
                                            ? guid.Value
                                            : "An error occurred when fetching guid",
                                    TypeEnums = new()
                                }
                            );
                        }
                        else
                        {
                            events.First(e => e.Guid == guid?.Value).TypeEnums.Add(eventType);
                        }
                    }
                );
        }

        LastFetched = events;
    }
}
