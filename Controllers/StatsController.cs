using Microsoft.AspNetCore.Mvc;
using WATPlanCore.Aggregators;
using WATPlanCore.Data;
using WATPlanCore.Models;

namespace WATPlanCore.Controllers;

[Route("api/stats")]
[ApiController]
public class StatsController : ControllerBase
{
    private readonly PlansDbContext _db;
    
    public StatsController(PlansDbContext dbContext)
    {
        _db = dbContext;
    }
    
    [HttpGet("top/{count:int}/{days:int}/{unique:bool}")]
    public IEnumerable<RankEntry> GetTopPlans(int count, int days, bool unique)
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        var now = TimeZoneInfo.ConvertTime(DateTime.Now, EventAggregator.Cest);
        var beginDate = days > 0 ?now.Subtract(TimeSpan.FromDays(days)) : DateTime.MinValue;

        var filtered = (!isDevelopment ? _db.History!.Where(h => !h.UserIp.StartsWith("127")) : _db.History!)
            .Where(h => h.CreatedDate > beginDate);
        var grouped = filtered.GroupBy(historyEntry => new {historyEntry.PlanId, historyEntry.PlanName, historyEntry.UserIp});
        var rank = grouped.Select(group => 
                new RankEntry 
                {
                    Count = group.Count(), 
                    PlanId = group.Key.PlanId!, 
                    PlanName = group.Key.PlanName
                })
            .OrderByDescending(rankEntry => rankEntry.Count)
            .Take(count > 0 ? count : int.MaxValue)
            .ToList();
        
        var pos = 1;
        foreach (var rankEntry in rank)
        {
            rankEntry.Position = pos++;
        }

        return rank;
    }
}