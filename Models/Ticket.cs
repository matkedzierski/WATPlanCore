using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WATPlanCore.ExternalServices.Aggregators;

namespace WATPlanCore.Models;

/// <summary>
/// Zgłoszenie do supportu WATPlan (składowane w bazie i przekazywane odpowiednio na maila)
/// </summary>
public class Ticket
{
    /// <summary>
    /// ID bazodanowe
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// ID w formacie WPYYMMDDXXXXX, gdzie: WP - skrót od WATPlan, YYMMDD - data (rok, miesiąc dzień), XXXXX - ID bazodanowe dopełnione zerami do 5 znaków.
    /// </summary>
    public string TicketId => CreatedDate.ToString("WPyyMMdd" + Id.ToString().PadLeft(5, '0'));

    /// <summary>
    /// Nazwa planu, którego dotyczy zgłoszenie (istotne przy kategoriach zgłoszenia związanych z konkretnymi planami)
    /// </summary>
    public string PlanName { get; set; }
    
    /// <summary>
    /// Treść zgłoszenia (opis użytkownika)
    /// </summary>
    public string Content { get; set; }
    
    /// <summary>
    /// E-mail zgłaszającego, na który udzielamy odpowiedzi
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Data utworzenia
    /// </summary>
    public DateTime CreatedDate { get; set; }
    
    /// <summary>
    /// Imię zgłaszającego
    /// </summary>
    public string? Sender { get; set; }

    [NotMapped]
    public TicketCategory Category { get; set; }
    
    [JsonIgnore]
    public string CategoryName
    {
        get => Category.ToString(); 
        set => Category = Enum.Parse<TicketCategory>(value);
    }

    public Ticket()
    {
    }

    public Ticket(string planName, string content, TicketCategory category, string email)
    {
        PlanName = planName;
        Content = content;
        Category = category;
        Email = email;
        CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, EventAggregator.Cest);
    }


    /// <summary>
    /// Kategoria zgłoszenia
    /// </summary>
    public enum TicketCategory
    {
        /// <summary>
        /// Brak pożądanego planu
        /// </summary>
        MissingPlan,
        /// <summary>
        /// Niepoprawne dane w planie
        /// </summary>
        IncorrectData,
        /// <summary>
        /// Błąd w działaniu aplikacji
        /// </summary>
        SiteError,
        /// <summary>
        /// Wsparcie techniczne
        /// </summary>
        TechSupport,
        /// <summary>
        /// Opinia
        /// </summary>
        Opinion,
        /// <summary>
        /// Sugestia
        /// </summary>
        Suggestion,
        /// <summary>
        /// Współpraca
        /// </summary>
        Cooperation,
        /// <summary>
        /// Inne
        /// </summary>
        Other
    }
}