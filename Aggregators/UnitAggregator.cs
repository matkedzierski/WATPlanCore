using System.Xml.Serialization;
using WATPlanCore.Models;

namespace WATPlanCore.Aggregators;

public static class UnitDataSource
{
    private const string StatusUrl = "http://www.plansoft.org/wat/status.xml";

    public static async Task<IEnumerable<Unit>?> GetAllUnits()
    {
        var status = await GetPlanSoftStatus();
        return status?.Units;
    }

    public static async Task<WatStatus?> GetPlanSoftStatus()
    {
        var serializer = new XmlSerializer(typeof(WatStatusXml));
        var client = new HttpClient { BaseAddress = new Uri(StatusUrl) };
        var stream = await client.GetStreamAsync(StatusUrl);
        var status = (WatStatusXml?) serializer.Deserialize(stream);
        return WatStatus.FromXml(status);
    }
}