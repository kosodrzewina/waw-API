using System.Xml.Linq;
using WawAPI.Models;

namespace WawAPI
{
    public class EventFetcher
    {
        private readonly Uri? _uri;

        public EventFetcher(string address)
        {
            if (IsUrlValid(address))
            {
                _uri = new Uri(address);
            }
            else
            {
                _uri = null;
                throw new Exception($"{address} is not a valid URL");
            }
        }

        public static bool IsUrlValid(string address)
        {
            return Uri.TryCreate(address, UriKind.Absolute, out Uri? uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        public async Task<ICollection<Event>?> Fetch()
        {
            if (_uri == null)
            {
                return null;
            }

            using var httpClient = new HttpClient();
            var httpResponseMessage = await httpClient.GetAsync(_uri);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var feed = XDocument.Load(_uri.ToString());
                
            return feed.Descendants()
            .Where(item => item.Name == "item")
            .Select(item => {
                var title = item.Element("title");
                var description = item.Element("description");
                var link = item.Element("link");
                var guid = item.Element("guid");

                return new Event
                {
                    Title = title != null && title.Value != null ?
                        title.Value :
                        "An error occurred when fetching title",
                    Description = description != null && description.Value != null ?
                        description.Value :
                        "An error occurred when fetching description",
                    Link = link != null && link.Value != null ?
                        link.Value :
                        "An error occurred when fetching link",
                    Guid = guid != null && guid.Value != null ?
                        guid.Value :
                        "An error occurred when fetching guid"
                };
            }).ToList();
        }
    }
}
