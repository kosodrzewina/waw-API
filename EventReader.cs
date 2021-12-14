using System.Xml;

namespace WawAPI
{
    public class EventReader
    {
        private readonly Uri? _uri;

        public EventReader(string address)
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
    }
}
