using System.Runtime.Serialization;

namespace WATPlanCore.Models;

[DataContract]
[Serializable]
public class Event
{
    public string?   Id          { get; set; } // Google ID
    public string?   Name        { get; set; } // Nazwa przedmiotu
    public string?   Shortcut    { get; set; } // Skrót nazwy
    public string?   Type        { get; set; } // Wykład, Ćwiczenia itd..
    public string?   TypeShortcut{ get; set; } // W, Ć, L ...
    public DateTime  StartTime   { get; set; } // data i godzina rozpoczęcia
    public DateTime  EndTime     { get; set; } // data i godzina zakończenia
    public string?   Number      { get; set; } // Numer zajęć 1, 2, 3-4, 5-8
    public string?   Lecturer    { get; set; } // Wykładowca
    public string?   Room        { get; set; } // Sala
    public string?   Groups      { get; set; } // Grupy
    public string?   Info        { get; set; } // Info dla studentów
    public string?   Color       { get; set; } // hex
    public int       Week        { get; set; } // 1-7
    public int       DayOfWeek   { get; set; } // 1-7
    public int       BlockNumber { get; set; } // 1-7
    public int       BlockSpan   { get; set; } // 1-7
    
    public string?   PlanId { get; set; }
    public string?   PlanName { get; set; }
        
                                                  
    public override string ToString()
    {
        return $"> Event: \n" +
               $"\tID          {Id         }\n" +
               $"\tName        {Name       }\n" +
               $"\tType        {Type       }\n" +
               $"\tLecturer    {Lecturer   }\n" +
               $"\tRoom        {Room       }\n" +
               $"\tGroups      {Groups     }\n" +
               $"\tInfo        {Info       }\n" +
               $"\tColor       {Color      }\n" +
               $"\tWeek        {Week       }\n" +
               $"\tDayOfWeek   {DayOfWeek  }\n" +
               $"\tBlockNumber {BlockNumber}\n" +
               $"\tBlockSpan   {BlockSpan  }\n" +
               $"\tPlanId      {PlanId  }\n" +
               $"\tPlanName    {PlanName  }\n" 
            ;
    }
}