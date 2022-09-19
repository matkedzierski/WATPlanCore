using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using WATPlanCore.Aggregators;
using WATPlanCore.Models;

namespace WATPlanCore.ExternalServices.Aggregators;

public class EventAggregator : IEventAggregator
{
    public static readonly TimeZoneInfo Cest = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

    private static readonly CalendarService Service = new(new BaseClientService.Initializer
    {
        ApiKey = @"AIzaSyCf7E62KovXoKaqlBPib41bVuYXzkUXXG8",
        ApplicationName = "WATPlan"
    });

    private static readonly string[] ColorValues =
    {
        //nowe (e-dziekanat)
        "#b8860b", "#00ffff", "#ffd700", "#ff6347", "#32cd32", "#ffa07a", "#ffebcd",
        "#ff69b4", "#ffdead", "#d8bfd8", "#afeeee", "#f0fff0", "#e6e6fa", "#ffc0cb", "#f08080",
        "#cd5c5c", "#adff2f", "#fffacd", "#e9967a",

        //stare
        "#32CD32", "#48D1CC", "#ADFF2F", "#A0A0A0", "#F09090", "#FFEBCD", "#886347",
        "#CD5C5C", "#EE82EE", "#FFA07A", "#AFEEEE", "#D8BFD8", "#B8860B", "#FFFACD",
        "#E6E6FA", "#FFD700", "#00FFFF", "#DEB887", "#90EE90", "#F08080", "#FF1493",
        "#E9967A", "#F0FFF0", "#FFDEAD", "#ADFF2F", "#FFEFD5", "#FF69B4", "#AAAAAA",
        "#883312"
    };

    public async Task<IEnumerable<Event>?> GetPlanEvents(string planId)
    {
        var request = new EventsResource.ListRequest(Service, planId + @"@group.calendar.google.com")
        {
            Fields = "summary, items(id, summary, description, start, end)", MaxResults = Int32.MaxValue
        };
        var gEvents = await request.ExecuteAsync();
        var eventColors = new Dictionary<string, string>();
        var eventNumbers = new Dictionary<string, int>();

        return from ge in gEvents.Items.OrderBy(e => e.Start.DateTime)
            where ge.Start.DateTime != null && ge.End.DateTime != null
            let details = GetEventDetails(ge.Description)
            let start = TimeZoneInfo.ConvertTime(ge.Start.DateTime.Value, Cest)
            let end = TimeZoneInfo.ConvertTime(ge.End.DateTime.Value, Cest)
            let name = GetEventName(ge.Summary)
            let sh = GetShortcut(name)
            let type = GetEventType(ge.Summary)
            select new Event(
                ge.Id,
                name,
                sh,
                type,
                GetShortcut(type),
                start,
                end,
                GetEventNumber(eventNumbers, name + type, start, end),
                details[0],
                details[1],
                details[3],
                details[2],
                GetEventColor(eventColors, name),
                planId,
                planName: PlanAggregator.GetPlanName(gEvents.Summary)
            );
    }

    private static string[] GetEventDetails(string desc)
    {
        const string w = "Wykładowca: ";
        const string s = "Sala: ";
        const string i = "Info dla studentów: ";
        const string g = "Grupa: ";
        var details = new[] { "", "", "", "" };
        var lines = desc.Split(Environment.NewLine.ToCharArray());

        foreach (var line in lines)
        {
            if (line.StartsWith(w)) details[0] = line[w.Length..];
            if (line.StartsWith(s)) details[1] = line[s.Length..];
            if (line.StartsWith(i)) details[2] = line[i.Length..];
            if (line.StartsWith(g)) details[3] = line[g.Length..];
        }

        return details;
    }

    private static string GetEventType(string summary)
    {
        var ind = summary.LastIndexOf('(');
        return ind > -1 ? summary.Substring(ind + 1, summary.Length - ind - 2) : summary;
    }

    private static string GetEventName(string summary)
    {
        if (summary.StartsWith("X") &&
            summary[1].ToString().ToUpper() ==
            summary[1].ToString()) // jeśli zaczyna się od X i druga litera jest wielka
            summary = summary[1..];

        var ind = summary.LastIndexOf('(');
        return ind > -1 ? summary[..ind] : summary;
    }

    private static int GetEventBlockSpan(DateTime start, DateTime end)
    {
        var s = end.Subtract(start);
        switch ((int)s.TotalMinutes)
        {
            case 95: return 1;
            case 205: return 2;
            case 315: return 3;
            case 425: return 4;
            case 560: return 5;
            case 670: return 6;
            case 780: return 7;
            default:
                var b = (int)s.TotalMinutes / 95;
                return b == 0 ? 1 : b;
        }
    }

    private static string GetEventColor(IDictionary<string, string> evColors, string name)
    {
        if (!evColors.ContainsKey(name))
            evColors.Add(name, ColorValues[evColors.Count]);
        return evColors[name];
    }

    private static string GetEventNumber(IDictionary<string, int> evNumbers, string name, DateTime start, DateTime end)
    {
        var span = GetEventBlockSpan(start, end);
        var nr = 1;
        if (evNumbers.ContainsKey(name))
        {
            nr = evNumbers[name] + 1;
            evNumbers[name] = nr + span - 1;
        }
        else
        {
            evNumbers[name] = span;
        }

        return span == 1 ? nr.ToString() : $"{nr}-{nr + span - 1}";
    }

    private static string GetShortcut(string name)
    {
        return name.Split(' ').Where(str => str.Length > 1 && str[0] != '(')
            .Aggregate("", (current, str) => current + str.ToUpper()[0]);
    }
}