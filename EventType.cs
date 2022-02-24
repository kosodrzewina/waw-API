namespace WawAPI;

public class EventType : Enumeration
{
    private const string _baseUrl = "https://waw4free.pl/rss";
    private const string _categoryUrl = $"{_baseUrl}-kategoria=";
    private const string _districtUrl = $"{_baseUrl}-dzielnica=";

    public string Address { get; private set; }

    public static readonly EventType
        Today = new(0, nameof(Today), $"{_baseUrl}-dzisiaj"),
        Latest = new(1, nameof(Latest), $"{_baseUrl}-ostatnie"),
        ForKids = new(2, nameof(ForKids), $"{_categoryUrl}dzieci"),
        Movies = new(3, nameof(Movies), $"{_categoryUrl}filmy"),
        Parties = new(4, nameof(Parties), $"{_categoryUrl}imprezy"),
        Concerts = new(5, nameof(Concerts), $"{_categoryUrl}koncerty"),
        Presentations = new(6, nameof(Presentations), $"{_categoryUrl}slajdowiska"),
        Walks = new(7, nameof(Walks), $"{_categoryUrl}spacery"),
        Sport = new(8, nameof(Sport), $"{_categoryUrl}sport"),
        Meetings = new(9, nameof(Meetings), $"{_categoryUrl}spotkania"),
        Standups = new(10, nameof(Standups), $"{_categoryUrl}standup"),
        Fairs = new(11, nameof(Fairs), $"{_categoryUrl}targi"),
        TheatrePlays = new(12, nameof(TheatrePlays), $"{_categoryUrl}teatr"),
        Workshops = new(13, nameof(Workshops), $"{_categoryUrl}warsztaty"),
        Lectures = new(14, nameof(Lectures), $"{_categoryUrl}wyklady"),
        Expositions = new(15, nameof(Expositions), $"{_categoryUrl}wystawy"),
        Other = new(16, nameof(Other), $"{_categoryUrl}inne"),
        English = new(17, nameof(English), $"{_categoryUrl}angielskie"),
        Online = new(18, nameof(Online), $"{_categoryUrl}online"),
        Bemowo = new(19, nameof(Bemowo), $"{_districtUrl}bemowo"),
        Bialoleka = new(20, nameof(Bialoleka), $"{_districtUrl}bialoleka"),
        Bielany = new(21, nameof(Bielany), $"{_districtUrl}bielany"),
        Mokotow = new(22, nameof(Mokotow), $"{_districtUrl}mokotow"),
        Ochota = new(23, nameof(Ochota), $"{_districtUrl}ochota"),
        PragaPoludnie = new(24, nameof(PragaPoludnie), $"{_districtUrl}pragapld"),
        PragaPolnoc = new(25, nameof(PragaPolnoc), $"{_districtUrl}pragapln"),
        Srodmiescie = new(26, nameof(Srodmiescie), $"{_districtUrl}srodmiescie"),
        Rembertow = new(27, nameof(Rembertow), $"{_districtUrl}rembertow"),
        Targowek = new(28, nameof(Targowek), $"{_districtUrl}targowek"),
        Ursus = new(29, nameof(Ursus), $"{_districtUrl}ursus"),
        Ursynow = new(30, nameof(Ursynow), $"{_districtUrl}ursynow"),
        Wawer = new(31, nameof(Wawer), $"{_districtUrl}wawer"),
        Wesola = new(32, nameof(Wesola), $"{_districtUrl}wesola"),
        Wilanow = new(33, nameof(Wilanow), $"{_districtUrl}wilanow"),
        Wlochy = new(34, nameof(Wlochy), $"{_districtUrl}wlochy"),
        Wola = new(35, nameof(Wola), $"{_districtUrl}wola"),
        Zoliborz = new(36, nameof(Zoliborz), $"{_districtUrl}zoliborz"),
        OutsideTheCity = new(37, nameof(OutsideTheCity), $"{_districtUrl}okolice");

    public EventType(int id, string name, string address) : base(id, name) => Address = address;
}
