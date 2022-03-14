using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WATPlanCore.Models.Json;

public class PlanInfoJson
{
    [JsonPropertyName("calendarName")]
    public string? CalendarName { get; set; }
    [JsonPropertyName("link")] 
    public string? Link { get; set; }
    [JsonPropertyName("calendarId")] 
    public string? CalendarId { get; set; }
}