namespace WawAPI;

public static class Extensions
{
    public static string GetUrl(this EventType eventType)
    {
        const string baseUrl = "https://waw4free.pl/rss";
        const string categoryUrl = $"{baseUrl}-kategoria=";
        const string districtUrl = $"{baseUrl}-dzielnica=";

        return eventType switch
        {
            EventType.Today => $"{baseUrl}-dzisiaj",
            EventType.Latest => $"{baseUrl}-ostatnie",
            EventType.ForKids => $"{categoryUrl}dzieci",
            EventType.Movies => $"{categoryUrl}filmy",
            EventType.Parties => $"{categoryUrl}imprezy",
            EventType.Concerts => $"{categoryUrl}koncerty",
            EventType.Presentations => $"{categoryUrl}slajdowiska",
            EventType.Walks => $"{categoryUrl}spacery",
            EventType.Sport => $"{categoryUrl}sport",
            EventType.Meetings => $"{categoryUrl}spotkania",
            EventType.Standups => $"{categoryUrl}standup",
            EventType.Fairs => $"{categoryUrl}targi",
            EventType.TheatrePlays => $"{categoryUrl}teatr",
            EventType.Workshops => $"{categoryUrl}warsztaty",
            EventType.Lectures => $"{categoryUrl}wyklady",
            EventType.Expositions => $"{categoryUrl}wystawy",
            EventType.Other => $"{categoryUrl}inne",
            EventType.English => $"{categoryUrl}angielskie",
            EventType.Online => $"{districtUrl}online",
            EventType.Bemowo => $"{districtUrl}bemowo",
            EventType.Bialoleka => $"{districtUrl}bialoleka",
            EventType.Bielany => $"{districtUrl}bielany",
            EventType.Mokotow => $"{districtUrl}mokotow",
            EventType.Ochota => $"{districtUrl}ochota",
            EventType.PragaPoludnie => $"{districtUrl}pragapld",
            EventType.PragaPolnoc => $"{districtUrl}pragapln",
            EventType.Srodmiescie => $"{districtUrl}srodmiescie",
            EventType.Rembertow => $"{districtUrl}rembertow",
            EventType.Targowek => $"{districtUrl}targowek",
            EventType.Ursus => $"{districtUrl}ursus",
            EventType.Ursynow => $"{districtUrl}ursynow",
            EventType.Wawer => $"{districtUrl}wawer",
            EventType.Wesola => $"{districtUrl}wesola",
            EventType.Wilanow => $"{districtUrl}wilanow",
            EventType.Wlochy => $"{districtUrl}wlochy",
            EventType.Wola => $"{districtUrl}wola",
            EventType.Zoliborz => $"{districtUrl}zoliborz",
            EventType.OutsideTheCity => $"{districtUrl}okolice",
            _ => throw new NotImplementedException()
        };
    }
}
