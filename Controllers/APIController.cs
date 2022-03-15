using Microsoft.AspNetCore.Mvc;
using WATPlanCore.Aggregators;
using WATPlanCore.Data;
using WATPlanCore.Models;

namespace WATPlanCore.Controllers;

[Route("api")]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly PlansDbContext _db;

        public ApiController(IHttpContextAccessor httpContextAccessor, PlansDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _db = dbContext;
    }
    
    [HttpGet("units")]
    public async Task<IEnumerable<Unit>> GetUnits()
    {
        var list = await UnitAggregator.GetAllUnits();
        return (list ?? Array.Empty<Unit>()).OrderByDescending(u =>  u.PlansCount);
    }

    [HttpGet("plans")]
    public async Task<IEnumerable<Plan>?> GetAllPlans()
    {
        var list = await PlanAggregator.GetAllPlans();
        return list?.OrderBy(p => p.Name?.ToLower());
    }
    
    [HttpGet("plans/{id}")]
    public async Task<IEnumerable<Plan>?> GetPlans(string id)
    {
        var list = await PlanAggregator.GetPlansForUnit(id);
        return list?.OrderBy(p => p.Name?.ToLower());
    }
    
    [HttpGet("events/{id}")]
    public async Task<IEnumerable<Event>?> GetEvents(string? id)
    {
        var list = await EventAggregator.GetPlanEvents(id);
        var entry = new HistoryEntry(id, _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress);
        _db.History?.Add(entry);
        await _db.SaveChangesAsync();
        return list;
    }
}