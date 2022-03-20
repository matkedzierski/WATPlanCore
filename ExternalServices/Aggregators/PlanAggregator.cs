using System.Text.Json;
using WATPlanCore.Models;
using WATPlanCore.Models.Json;

namespace WATPlanCore.Aggregators;

public static class PlanAggregator
{

    private const string PlanPrefix = "http://plansoft.org/wat/";
    public static async Task<IEnumerable<Plan>?> GetPlansForUnit(string? unitId)
    {
        Console.WriteLine($"Getting plans for unit {unitId}");
        var client = new HttpClient { BaseAddress = new Uri(PlanPrefix) };
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

    public static async Task<IEnumerable<Plan>> GetAllPlans()
    {
        Console.WriteLine($"Getting all plans");
        var units = await UnitAggregator.GetAllUnits();
        var list = new List<Plan>();
        if (units == null) return list;
        foreach (var unit in units)
        {
            list.AddRange(await GetPlansForUnit(unit.Id!) ?? Array.Empty<Plan>());
        }

        return list;
    }
}