using System.Runtime.Serialization;

namespace WATPlanCore.Models;

[DataContract]
[Serializable]
public class Plan
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    private Dictionary<string, string> EventColors { get; set; } //Name, Color

    public Plan()
    {
        EventColors = new Dictionary<string, string>();
    }
        
    public Plan(string? id)
    {
        EventColors = new Dictionary<string, string>();
        Id = id;
    }

    public override string ToString()
    {
        return $"> Plan: \n" + 
               $"Nazwa:{Name} \n" +
               $"ID:    {Id} \n" +
               $"Typ:   {Type} \n";
    }
}