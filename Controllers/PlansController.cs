using System.Net;
using Microsoft.AspNetCore.Mvc;
using WATPlanCore.Aggregators;
using WATPlanCore.Data;
using WATPlanCore.ExternalServices.Aggregators;
using WATPlanCore.Models;

namespace WATPlanCore.Controllers;

/// <summary>
/// Umożliwia dostęp do listy jednostek, planów i wydarzeń.
/// </summary>
[Produces("application/json")]
[Route("api")]
[ApiController]
public class PlansController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly PlansDbContext _db;

    public PlansController(IHttpContextAccessor httpContextAccessor, PlansDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _db = dbContext;
    }
        
        
    /// <summary>
    /// Udostępnia listę wszystkich jednostek (wydziałów) uczelni.
    /// </summary>
    /// <returns>Lista jednostek (Unit[])</returns>
    [HttpGet("units")]
    public async Task<IEnumerable<Unit>> GetUnits()
    {
        var list = await UnitAggregator.GetAllUnits();
        return (list ?? Array.Empty<Unit>()).OrderByDescending(u =>  u.PlansCount);
    }
    
    
    /// <summary>
    /// Udostępnia listę planów ze wszystkich jednostek łącznie.
    /// </summary>
    /// <returns>Lista wszystkich planów uczelni (Plan[]).</returns>
    /// <remarks>UWAGA! Ta operacja może chwilę potrwać, ale zawiera kompletną listę. Aby czas oczekiwania na wynik był krótszy, sprawdź metodę <see cref="GetUnitPlans"><code>/api/plans/{id}</code></see> - lista planów dla wybranego wydziału.</remarks>
    [HttpGet("plans")]
    public async Task<IEnumerable<Plan>?> GetAllPlans()
    {
        var list = await PlanAggregator.GetAllPlans();
        return list?.OrderBy(p => p.Name?.ToLower());
    }
    
    /// <summary>
    /// Udostępnia listę planów wybranej jednostki.
    /// </summary>
    /// <param name="id">3-literowy identyfikator jednostki (np. WCY), case insensitive.</param>
    /// <returns>Lista planów jednostki (Plan[])</returns>
    [HttpGet("plans/{id}")]
    public async Task<IEnumerable<Plan>?> GetUnitPlans(string id)
    {
        var list = await PlanAggregator.GetPlansForUnit(id);
        return list?.OrderBy(p => p.Name?.ToLower());
    }

    /// <summary>
    /// Udostępnia wydarzenia
    /// </summary>
    /// <param name="id">Identyfikator planu (Plan.id), na przykład 9ekbij6tsrv2bqbbgmcpa9r8g0</param>
    /// <returns>Lista wydarzeń wybranego planu (Event[])</returns>
    [HttpGet("events/{id}")]
    public async Task<IEnumerable<Event>?> GetEvents(string? id)
    {
        var userAddress = _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!;
        var list = (await EventAggregator.GetPlanEvents(id))!.ToList();
        
        if (list.Count <= 0) return list;
        
        var entry = new HistoryEntry(id, userAddress.ToString(), list[0].PlanName);
        _db.History?.Add(entry);
        await _db.SaveChangesAsync();

        return list;
    }
}