using System.Runtime.Serialization;

namespace WATPlanCore.Models;

/// <summary>
/// Reprezentacja uczelnianego planu zajęć
/// </summary>
[DataContract]
[Serializable]
public class Plan
{
    /// <summary>
    /// ID planu powiązane z Google Calendar ID
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// Nazwa planu
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Typ planu - np. Grupa, Zasób, kiedyś jeszcze Wykładowca
    /// </summary>
    public string Type { get; set; }
    private Dictionary<string, string> EventColors { get; set; } //Name, Color

    public Plan()
    {
        EventColors = new Dictionary<string, string>();
    }
        
    public Plan(string? id, string name, string type)
    {
        EventColors = new Dictionary<string, string>();
        Id = id;
        Name = name;
        Type = type;
    }

    public override string ToString()
    {
        return $"> Plan: \n" + 
               $"Nazwa:{Name} \n" +
               $"ID:    {Id} \n" +
               $"Typ:   {Type} \n";
    }
}