using System.Text.Json;
using Microsoft.Extensions.Options;
using WATPlanCore.ExternalServices.Aggregators;
using WATPlanCore.Models;
using WATPlanCore.Models.Json;
using WATPlanCore.Models.Settings;

namespace WATPlanCore.Aggregators;

public class PlanAggregator : IPlanAggregator
{
    private readonly PlanSoftSettings _planSoftSettings;
    private readonly IUnitAggregator _unitAggregator;

    public PlanAggregator(IUnitAggregator unitAggregator, IOptions<PlanSoftSettings> planSoftSettings)
    {
        _unitAggregator = unitAggregator;
        _planSoftSettings = planSoftSettings.Value;
    }

    public async Task<IEnumerable<Plan>?> GetPlansForUnit(string? unitId)
    {
        Console.WriteLine($"Getting plans for unit {unitId}");
        var client = new HttpClient { BaseAddress = new Uri(_planSoftSettings.BaseUnitUrl) };
        string json;
        try
        {
            json = await client.GetStringAsync(unitId + ".js");
        }
        catch (HttpRequestException)
        {
            return new List<Plan>();
        }
        json = json[11..^1]
            .Replace("\"", "\\\"")
            .Replace("'", "\"")
            .Replace("calendarName", "\"calendarName\"")
            .Replace("calendarId", "\"calendarId\"")
            .Replace("link", "\"link\"");
        try
        {
            var list = JsonSerializer.Deserialize<List<PlanInfoJson>>(json);
            return list?.ConvertAll(info => new Plan
            {
                Id = GetPlanId(info.CalendarId),
                Name = GetPlanName(info.CalendarName),
                Type = GetPlanType(info.CalendarName)
            });
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Couldn't read json for unit {unitId}");
            Console.WriteLine("Details:");
            Console.WriteLine(e.Message);
            
            return new List<Plan>();
        }
    }

    private static string? GetPlanId(string? calendarId)
    {
        return calendarId?[..calendarId.IndexOf("@", StringComparison.Ordinal)];
    }

    public static string GetPlanName(string calendarName)
    {
        return calendarName[..calendarName.LastIndexOf(" ", StringComparison.Ordinal)];
    }
    private static string? GetPlanType(string? calendarName)
    {
        return calendarName?[(calendarName.LastIndexOf(" ", StringComparison.Ordinal)+1)..];
    }

    public async Task<IEnumerable<Plan>> GetAllPlans()
    {
        Console.WriteLine($"Getting all plans");
        var units = await _unitAggregator.GetAllUnits();
        var list = new List<Plan>();
        if (units == null) return list;
        foreach (var unit in units)
        {
            list.AddRange(await GetPlansForUnit(unit.Id) ?? Array.Empty<Plan>());
        }

        return list;
    }
}