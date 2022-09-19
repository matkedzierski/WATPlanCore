using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using WATPlanCore.Models;
using WATPlanCore.Models.Settings;
using WATPlanCore.Models.Xml;

namespace WATPlanCore.ExternalServices.Aggregators;

public class UnitAggregator : IUnitAggregator
{
    private readonly PlanSoftSettings _PlanSoftSettings;

    public UnitAggregator(IOptions<PlanSoftSettings> planSoftSettings)
    {
        _PlanSoftSettings = planSoftSettings.Value;
    }

    public async Task<IEnumerable<Unit>?> GetAllUnits()
    {
        var status = await GetPlanSoftStatus();
        return status?.Units;
    }

    public async Task<WatStatus?> GetPlanSoftStatus()
    {
        var serializer = new XmlSerializer(typeof(WatStatusXml));
        var client = new HttpClient { BaseAddress = new Uri(_PlanSoftSettings.StatusUrl) };
        var stream = await client.GetStreamAsync("");
        var status = (WatStatusXml?) serializer.Deserialize(stream);
        return WatStatus.FromXml(status);
    }
}