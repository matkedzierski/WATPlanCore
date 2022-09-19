using WATPlanCore.Models;

namespace WATPlanCore.ExternalServices.Aggregators;

public interface IEventAggregator
{
    public Task<IEnumerable<Event>?> GetPlanEvents(string planId);
}