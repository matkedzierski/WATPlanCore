using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WATPlanCore.Models;

/// <summary>
/// Reprezentacja jednych zajęć. Może trwać 1 lub więcej bloków. 
/// </summary>
[DataContract]
[Serializable]
public class Event
{
    public Event(string id, string name, string shortcut, string type, string typeShortcut, DateTime startTime, DateTime endTime, string number, string lecturer, string room, string groups, string info, string color, string planId, string planName)
    {
        Id = id;
        Name = name;
        Shortcut = shortcut;
        Type = type;
        TypeShortcut = typeShortcut;
        StartTime = startTime;
        EndTime = endTime;
        Number = number;
        Lecturer = lecturer;
        Room = room;
        Groups = groups;
        Info = info;
        Color = color;
        PlanId = planId;
        PlanName = planName;
    }

    /// <summary>
    /// ID wydarzenia powiązane z Google Calendar ID
    /// </summary>]
    public string   Id          { get; set; }
    
    /// <summary>
    /// Nazwa przedmiotu
    /// </summary>
    public string   Name        { get; set; }
    
    /// <summary>
    /// Skrót nazwy (wyznaczany jako pierwsze litery kolejnych wyrazów dłuższych niż 1 znak)
    /// </summary>
    public string   Shortcut    { get; set; }
    
    /// <summary>
    /// Rodzaj zajęć (Wykład, Ćwiczenia itd.) 
    /// </summary>
    public string   Type        { get; set; }
    
    /// <summary>
    /// Skrót rodzaju zajęć (W, Ć itd.)
    /// </summary>
    public string   TypeShortcut{ get; set; }
    
    /// <summary>
    /// Czas rozpoczęcia
    /// </summary>
    public DateTime  StartTime   { get; set; }
    
    /// <summary>
    /// Czas zakończenia
    /// </summary>
    public DateTime  EndTime     { get; set; }
    
    /// <summary>
    /// Numer zajęć (zliczane kolejne wystąpienia w planie, np. 1, 2, 3-4, 5-8)
    /// </summary>
    public string   Number      { get; set; }
    
    /// <summary>
    /// Wykładowca prowadzący zajęcia (obecnie niedostępne)
    /// </summary>
    [JsonIgnore]
    public string   Lecturer    { get; set; }
    
    /// <summary>
    /// Sala wykładowa
    /// </summary>
    public string   Room        { get; set; }
    
    /// <summary>
    /// Grupy uczestniczące w zajęciach
    /// </summary>
    public string   Groups      { get; set; }
    
    /// <summary>
    /// Informacje dla studentów (czasami wpisywane przez planistów lub prowadzących)
    /// </summary>
    public string   Info        { get; set; }
    
    /// <summary>
    /// Kolor wydarzenia (spośród stałej puli, jeden przedmiot = jeden kolor)
    /// </summary>
    public string   Color       { get; set; }
    
    /// <summary>
    /// ID planu (Google Calendar ID), do którego należy wydarzenie
    /// </summary>
    public string   PlanId      { get; set; }
    
    /// <summary>
    /// Nazwa planu, do którego należy wydarzenie
    /// </summary>
    public string   PlanName    { get; set; }
        
                                                  
    public override string ToString()
    {
        return "> Event: \n" +
               $"\tID          {Id         }\n" +
               $"\tName        {Name       }\n" +
               $"\tType        {Type       }\n" +
               $"\tLecturer    {Lecturer   }\n" +
               $"\tRoom        {Room       }\n" +
               $"\tGroups      {Groups     }\n" +
               $"\tInfo        {Info       }\n" +
               $"\tColor       {Color      }\n" +
               $"\tPlanId      {PlanId     }\n" +
               $"\tPlanName    {PlanName   }\n" 
            ;
    }
}