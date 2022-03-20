using System.ComponentModel.DataAnnotations;
using System.Net;
using WATPlanCore.Aggregators;
using WATPlanCore.ExternalServices.Aggregators;

namespace WATPlanCore.Models;

public class HistoryEntry
{
    [Key]
    public long Id { get; set; }

    public string? PlanId { get; set; }
    public string? PlanName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UserIp { get; set; }

    public HistoryEntry(string? planId, string userIp, string? planName)
    {
        PlanId = planId;
        UserIp = userIp;
        PlanName = planName;
        CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, EventAggregator.Cest);
    }

    public HistoryEntry()
    {
    }
}