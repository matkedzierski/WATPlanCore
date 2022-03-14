using WATPlanCore.Models.Xml;

namespace WATPlanCore.Models;

public class WatStatus
{
    public IEnumerable<Unit>? Units { get; set; }
    private string? LastUpdateText { get; set; }

    public static WatStatus? FromXml(WatStatusXml? xml)
    {
        if (xml == null) return null;
        var status = new WatStatus
        {
            Units = xml.Data?.Folders?.ConvertAll(folder => new Unit
            {
                Id = folder.Name, 
                PlansCount = folder.ProcessedCnt, 
                Name = UnitNames[folder.Name!]
            }),
            LastUpdateText = xml.Who?.LastUpdateText
        };
        return status;
    }


    private static readonly Dictionary<string, string> UnitNames = new()
    {
        { "DOK","Dział Organizacji Kształcenia" },
        { "IOE","Instytut Optoelektroniki" },
        { "SDR","Szkoła Doktorska WAT" },
        { "SSW","Studium Szkolenia Wojskowego" },
        { "SWF","Studium Wychowania Fizycznego" },
        { "WCY","Wydział Cybernetyki" },
        { "WEL","Wydział Elektroniki" },
        { "WIG","Wydział Inżynierii Lądowej i Geodezji" },
        { "WLO","Wydział Bezpieczeństwa, Logistyki i Zarządzania" },
        { "WME","Wydział Inżynierii Mechanicznej" },
        { "WML","Wydział Mechatroniki, Uzbrojenia i Lotnictwa" },
        { "WTC","Wydział Nowych Technologii i Chemii" } };
}