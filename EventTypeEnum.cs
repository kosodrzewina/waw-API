namespace WawAPI;

public class EventTypeEnum : Enumeration
{
    private const string _baseUrl = "https://waw4free.pl/rss";
    private const string _categoryUrl = $"{_baseUrl}-kategoria=";
    private const string _districtUrl = $"{_baseUrl}-dzielnica=";

    public string Address { get; private set; }

    public static readonly EventTypeEnum Today = new(1, nameof(Today), $"{_baseUrl}-dzisiaj"),
        Latest = new(2, nameof(Latest), $"{_baseUrl}-ostatnie"),
        ForKids = new(3, nameof(ForKids), $"{_categoryUrl}dzieci"),
        Movies = new(4, nameof(Movies), $"{_categoryUrl}filmy"),
        Parties = new(5, nameof(Parties), $"{_categoryUrl}imprezy"),
        Concerts = new(6, nameof(Concerts), $"{_categoryUrl}koncerty"),
        Presentations = new(7, nameof(Presentations), $"{_categoryUrl}slajdowiska"),
        Walks = new(8, nameof(Walks), $"{_categoryUrl}spacery"),
        Sport = new(9, nameof(Sport), $"{_categoryUrl}sport"),
        Meetings = new(10, nameof(Meetings), $"{_categoryUrl}spotkania"),
        Standups = new(11, nameof(Standups), $"{_categoryUrl}standup"),
        Fairs = new(12, nameof(Fairs), $"{_categoryUrl}targi"),
        TheatrePlays = new(13, nameof(TheatrePlays), $"{_categoryUrl}teatr"),
        Workshops = new(14, nameof(Workshops), $"{_categoryUrl}warsztaty"),
        Lectures = new(15, nameof(Lectures), $"{_categoryUrl}wyklady"),
        Expositions = new(16, nameof(Expositions), $"{_categoryUrl}wystawy"),
        Other = new(17, nameof(Other), $"{_categoryUrl}inne"),
        English = new(18, nameof(English), $"{_categoryUrl}angielskie"),
        Online = new(19, nameof(Online), $"{_categoryUrl}online"),
        Bemowo = new(20, nameof(Bemowo), $"{_districtUrl}bemowo"),
        Bialoleka = new(21, nameof(Bialoleka), $"{_districtUrl}bialoleka"),
        Bielany = new(22, nameof(Bielany), $"{_districtUrl}bielany"),
        Mokotow = new(23, nameof(Mokotow), $"{_districtUrl}mokotow"),
        Ochota = new(24, nameof(Ochota), $"{_districtUrl}ochota"),
        PragaPoludnie = new(25, nameof(PragaPoludnie), $"{_districtUrl}pragapld"),
        PragaPolnoc = new(26, nameof(PragaPolnoc), $"{_districtUrl}pragapln"),
        Srodmiescie = new(27, nameof(Srodmiescie), $"{_districtUrl}srodmiescie"),
        Rembertow = new(28, nameof(Rembertow), $"{_districtUrl}rembertow"),
        Targowek = new(29, nameof(Targowek), $"{_districtUrl}targowek"),
        Ursus = new(30, nameof(Ursus), $"{_districtUrl}ursus"),
        Ursynow = new(31, nameof(Ursynow), $"{_districtUrl}ursynow"),
        Wawer = new(32, nameof(Wawer), $"{_districtUrl}wawer"),
        Wesola = new(33, nameof(Wesola), $"{_districtUrl}wesola"),
        Wilanow = new(34, nameof(Wilanow), $"{_districtUrl}wilanow"),
        Wlochy = new(35, nameof(Wlochy), $"{_districtUrl}wlochy"),
        Wola = new(36, nameof(Wola), $"{_districtUrl}wola"),
        Zoliborz = new(37, nameof(Zoliborz), $"{_districtUrl}zoliborz"),
        OutsideTheCity = new(38, nameof(OutsideTheCity), $"{_districtUrl}okolice");

    public EventTypeEnum(int id, string name, string address) : base(id, name) => Address = address;
}
