using System.Runtime.Serialization;

namespace WATPlanCore.Models;

/// <summary>
/// Reprezentacja jednostki organizacyjnej WAT, najczęściej wydział
/// </summary>
[DataContract]
[Serializable]
public class Unit
{
    /// <summary>
    /// ID jednostki - 3-literowy skrót, np. WCY
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// Pełna nazwa jednostki
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Liczba planów w jednostce
    /// </summary>
    public int PlansCount { get; set; }
}