using System.Runtime.Serialization;

namespace WATPlanCore.Models;

[DataContract]
[Serializable]
public class Unit
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public int PlansCount { get; set; }
}