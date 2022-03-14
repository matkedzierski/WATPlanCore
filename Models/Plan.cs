using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Google.Apis.Calendar.v3;
using Newtonsoft.Json;
using WATPlan.API;

namespace WATPlan.Models
{
    [DataContract]
    [Serializable]
    public class PlanModel
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Type { get; set; }
        [IgnoreDataMember] 
        private List<EventModel> Events { get; set; }
        [IgnoreDataMember] 
        private Dictionary<string, string> EventColors { get; set; } //Name, Color
        
        public PlanModel(){}
        
        public PlanModel(string id)
        {
            EventColors = new Dictionary<string, string>();
            ID = id;
            SetupPlanByID();
            Events = EventsController.LoadEventsForPlan(ID);
        }

        public override string ToString()
        {
            return $"> Plan: \n" + 
                   $"Nazwa:{Name} \n" +
                   $"ID:    {ID} \n" +
                   $"Typ:   {Type} \n";
        }

        public void SetupPlanByID()
        {
            var request = new EventsResource.ListRequest(Global.service, ID + @"@group.calendar.google.com")
            {
                Fields = "summary, items(id, summary, description, start, end)"
            };
            var gEvents = request.Execute();
            
            var sum = gEvents.Summary;
            var ind = sum.LastIndexOf(' ');
            Name = sum.Substring(0, ind);
            Type = sum.Substring(ind + 1);
        }

        public IEnumerable<EventModel> GetWeekEvents(int WeekOffset)
        {
            //wyznacz ktory tydzien
            var now = TimeZoneInfo.Local.Equals(Global.cest) ? DateTime.Now : TimeZoneInfo.ConvertTime(DateTime.Now, Global.cest);
            var currentWeek = EventsController.GetEventWeekNumber(now);
            var week = currentWeek + WeekOffset;
            //zbierz wydarzenia z tego tygodnia
            return Events.Where(e => e.Week == week).ToList().OrderByDescending(model => model.Week*7 + model.DayOfWeek);
        }
        
    }
}