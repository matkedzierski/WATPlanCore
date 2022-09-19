using WATPlanCore.Models;

namespace WATPlanCore.ExternalServices.Aggregators;

public interface IUnitAggregator
{
    public Task<IEnumerable<Unit>?> GetAllUnits();
    public Task<WatStatus?> GetPlanSoftStatus();
}