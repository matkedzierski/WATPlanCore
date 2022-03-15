using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WATPlanCore.Models;

public class HistoryEntry
{
    [Key]
    public Guid Id { get; set; }

    public string? PlanId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public IPAddress? UserIp { get; set; }

    public HistoryEntry(string? planId, IPAddress? userIp)
    {
        PlanId = planId;
        UserIp = userIp;
        CreatedDate = DateTime.Now;
    }

    public HistoryEntry()
    {
    }
}