using System.Text.Json;
using WATPlanCore.Models;
using WATPlanCore.Models.Json;

namespace WATPlanCore.Aggregators;

public static class PlanAggregator
{

    private const string PlanPrefix = "http://plansoft.org/wat/";
    public static async Task<IEnumerable<Plan>?> GetPlansForUnit(string unitId)
    {
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
            .Replace("'", "\"")
            .Replace("calendarName", "\"calendarName\"")
            .Replace("calendarId", "\"calendarId\"")
            .Replace("link", "\"link\"");
        var list = JsonSerializer.Deserialize<List<PlanInfoJson>>(json);
        return list?.ConvertAll(info => new Plan
            {
                Id = GetPlanId(info.CalendarId), 
                Name = GetPlanName(info.CalendarName), 
                Type = GetPlanType(info.CalendarName)
            });
    }

    private static string? GetPlanId(string? calendarId)
    {
        return calendarId?[..(calendarId.IndexOf("@", StringComparison.Ordinal) + 1)];
    }

    private static string? GetPlanName(string? calendarName)
    {
        return calendarName?[..calendarName.LastIndexOf(" ", StringComparison.Ordinal)];
    }
    private static string? GetPlanType(string? calendarName)
    {
        return calendarName?[(calendarName.LastIndexOf(" ", StringComparison.Ordinal)+1)..];
    }
}