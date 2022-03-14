using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using WATPlanCore.Models;

namespace WATPlanCore.Aggregators;

public static class EventAggregator
{
    private static readonly TimeZoneInfo Cest = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

    private static readonly CalendarService Service = new(new BaseClientService.Initializer
    {
        ApiKey = @"AIzaSyCf7E62KovXoKaqlBPib41bVuYXzkUXXG8",
        ApplicationName = "WATPlan"
    });

    private static readonly string[] ColorValues = { "#32CD32", "#48D1CC", "#ADFF2F", "#A0A0A0", "#F09090", "#FFEBCD", "#886347", "#CD5C5C", "#EE82EE", "#FFA07A", "#AFEEEE", "#D8BFD8", "#B8860B", "#FFFACD", "#E6E6FA", "#FFD700", "#32CD32", "#00FFFF", "#DEB887", "#90EE90", "#F08080", "#FF1493", "#E9967A", "#F0FFF0", "#FFDEAD", "#ADFF2F", "#FFEFD5", "#FF69B4", "#AAAAAA", "#883312", "#32CD32", "#48D1CC", "#ADFF2F", "#A0A0A0", "#F09090", "#FFEBCD", "#FF6347", "#CD5C5C", "#EE82EE", "#FFA07A", "#AFEEEE", "#D8BFD8", "#B8860B", "#FFFACD", "#E6E6FA", "#FFD700", "#32CD32", "#00FFFF", "#DEB887", "#90EE90", "#F08080", "#FF1493", "#E9967A", "#F0FFF0", "#FFDEAD", "#ADFF2F", "#FFEFD5", "#FF69B4", "#AAAAAA", "#883312" };
    
    public static async Task<IEnumerable<Event>?> GetPlanEvents(string planId)
    {
        var currDate = DateTime.Now;
            var startOfAcademicYear = new DateTime(currDate.Year, 10, 1);
            if (currDate.Month < 10) startOfAcademicYear = startOfAcademicYear.AddYears(-1);
            
            var ret = new List<Event>();
            var request = new EventsResource.ListRequest(Service, planId + @"@group.calendar.google.com")
            {
                Fields = "summary, items(id, summary, description, start, end)", MaxResults = 9999
            };
            var gEvents = await request.ExecuteAsync();
            var eventColors = new Dictionary<string, string>();
            
            foreach (var ge in gEvents.Items)
            {
                if (ge.Start.DateTime == null || ge.End.DateTime == null) continue;
                if(ge.Start.DateTime.Value < startOfAcademicYear) continue;
                var details = GetEventDetails(ge.Description);
                var start = TimeZoneInfo.ConvertTime(ge.Start.DateTime.Value, Cest);
                var end = TimeZoneInfo.ConvertTime(ge.End.DateTime.Value, Cest);
                var name = GetEventName(ge.Summary);
                var sh = GetShortcut(name);
                var type = GetEventType(ge.Summary);
                var ev = new Event
                {
                    Id = ge.Id,
                    Name = name,
                    Shortcut = sh,
                    Type = type,
                    TypeShortcut = GetShortcut(type),
                    StartTime = start,
                    EndTime = end,
                    Lecturer = details[0],
                    Room = details[1],
                    Groups = details[3],
                    Info = details[2],
                    Color = GetEventColor(eventColors, name),
                    Week = GetEventWeekNumber(start),
                    DayOfWeek = GetEventDayOfWeek(start),
                    BlockNumber = GetEventBlockNumber(start),
                    BlockSpan = GetEventBlockSpan(start, end)
                };
                ret.Add(ev);
            }
            var eventNumbers = new Dictionary<string, int>();
            foreach (var evt in ret.OrderBy(e => e.Week).ThenBy(e => e.DayOfWeek).ThenBy(e => e.BlockNumber)) 
            {
                evt.Number = GetEventNumber(eventNumbers, evt.Name + evt.Type, evt.BlockSpan);
            }
            return ret;
        }

        private static int GetEventDayOfWeek(DateTime start)
        {
            var d = (int) start.DayOfWeek;
            if (d == 0) d = 7;
            return d;
        }

        private static string[] GetEventDetails(string desc)
        {
            const string w = "Wykładowca: "; 
            const string s = "Sala: ";
            const string i = "Info dla studentów: ";
            const string g = "Grupa: ";
            var details = new[] {"", "", "", ""};
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
            return ind>-1 ? summary.Substring(ind+1, summary.Length-ind-2) : summary;
        }
        
        private static string GetEventName(string summary)
        {
            if (summary.StartsWith("X") && summary[1].ToString().ToUpper() == summary[1].ToString()) // jeśli zaczyna się od X i druga litera jest wielka
            {
                summary = summary[1..];
            }
            var ind = summary.LastIndexOf('(');
            return ind>-1 ? summary[..ind] : summary;
        }

        private static int GetEventWeekNumber(DateTime start)
        {
            // wyznacz ostatni pierwszy października
            var currDate = DateTime.Now.Date;
            var startOfAcademicYear = new DateTime(currDate.Year, 10, 1);
            if (currDate.Month < 10) startOfAcademicYear = startOfAcademicYear.AddYears(-1);
            
            // wyznacz pierwszy poniedziałek <= 1.10
            var dow = (int)startOfAcademicYear.DayOfWeek;
            if (dow == 0) dow = 7; // 0 to niedziela -> 7
            var fromMonday = dow - 1; // ile dni od poniedzialku
            var mondayZero = startOfAcademicYear.AddDays(-1 * fromMonday); // wyznacz datę pierwszego poniedziałku przed 1.10
            
            // wyznacz numer tygodnia względem zerowego poniedziałku
            var timespan = start.Date.Subtract(mondayZero); // odleglosc wydarzenia od poniedzialku zerowego
            var week = (int)timespan.TotalDays / 7; // na tygodnie

            return week;

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
                    var b = (int) s.TotalMinutes/95 ;
                    return b==0? 1 : b;
            }
        }

        private static int GetEventBlockNumber(DateTime start)
        {
            /* 08:00 09:50 11:40 13:30 15:45 17:35 19:25 */
            return start.Hour switch
            {
                8 => 1,
                9 => 2,
                11 => 3,
                13 => 4,
                15 => 5,
                17 => 6,
                19 => 7,
                _ => -1
            };
        }
        
        private static string GetEventColor(IDictionary<string, string> evColors, string name)
        {
            if (!evColors.ContainsKey(name))
                evColors.Add(name, ColorValues[evColors.Count]);
            return evColors[name];
        }
        
        private static string GetEventNumber(IDictionary<string, int> evNumbers, string name, int span)
        {
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
            return name.Split(' ').Where(str => str.Length > 1 && str[0] != '(').Aggregate("", (current, str) => current + str.ToUpper()[0]);
        }
}