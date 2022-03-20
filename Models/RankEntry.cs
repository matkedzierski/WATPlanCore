namespace WATPlanCore.Models;

/// <summary>
/// Pojedynczy wpis rankingu wyznaczany na podstawie historii odwiedzin
/// </summary>
public class RankEntry
{
    /// <summary>
    /// Pozycja w rankingu
    /// </summary>
    public int Position { get; set; }
    
    /// <summary>
    /// Nazwa planu
    /// </summary>
    public string PlanName { get; set; }
    
    /// <summary>
    /// ID planu
    /// </summary>
    public string PlanId { get; set; }
    
    /// <summary>
    /// Liczba odwiedzin planu
    /// </summary>
    public int Count { get; set; }
}