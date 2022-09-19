using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WATPlanCore.Data;
using WATPlanCore.ExternalServices.Aggregators;
using WATPlanCore.Models;

namespace WATPlanCore.Controllers;

/// <summary>
/// Umożliwia dostęp do statystyk użycia WATPlan'u.
/// </summary>
[Produces("application/json")]
[Route("api/stats")]
[ApiController]
[EnableCors("watplan")]
public class StatsController : ControllerBase
{
    private readonly PlansDbContext _db;
    
    public StatsController(PlansDbContext dbContext)
    {
        _db = dbContext;
    }
    
    /// <summary>
    /// Zwraca ranking - liczbę odwiedzin poszczególnych planów z wybranej liczby ostatnich dni.
    /// </summary>
    /// <param name="count">Maks. liczba planów uwzględniona w zestawieniu (np. TOP 100 planów) - dla 0 uwzględnia wszystkie.</param>
    /// <param name="days">Maks. liczba ostatnich dni z których dane mają być uwzględnione - dla 0 od początku.</param>
    /// <param name="unique">Sprawdzanie tylko unikalnych odwiedzin (dostępne wkrótce)</param>
    /// <returns>Lista wpisów rankingowych (RankEntry[])</returns>
    [HttpGet("top/{count:int}/{days:int}/{unique:bool}")]
    public IEnumerable<RankEntry> GetTopPlans(int count, int days, bool unique)
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        var now = TimeZoneInfo.ConvertTime(DateTime.Now, EventAggregator.Cest);
        var beginDate = days > 0 ?now.Subtract(TimeSpan.FromDays(days)) : DateTime.MinValue;

        var filtered = (!isDevelopment ? _db.History!.Where(h => !h.UserIp.StartsWith("127")) : _db.History!)
            .Where(h => h.CreatedDate > beginDate);
        var grouped = filtered.GroupBy(historyEntry => new {historyEntry.PlanId, historyEntry.PlanName});
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