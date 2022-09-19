using WATPlanCore.Models;

namespace WATPlanCore.ExternalServices.Aggregators;

public interface IPlanAggregator
{
    public Task<IEnumerable<Plan>?> GetPlansForUnit(string? unitId);
    public Task<IEnumerable<Plan>> GetAllPlans();
}