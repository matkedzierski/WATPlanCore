using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using WATPlanCore.Aggregators;

namespace WATPlanCore.Models;

public class Ticket
{
    [Key]
    public long Id { get; set; }

    public string TicketId => CreatedDate.ToString("WPyyMMdd" + Id.ToString().PadLeft(5, '0'));

    public string PlanName { get; set; }
    public string Content { get; set; }
    public string Email { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Sender { get; set; }

    [NotMapped]
    [JsonIgnore]
    public TicketCategory CategoryEnum { get; set; }
    public string Category
    {
        get => CategoryEnum.ToString(); 
        set => CategoryEnum = Enum.Parse<TicketCategory>(value);
    }

    public Ticket()
    {
    }

    public Ticket(string planName, string content, TicketCategory categoryEnum, string email)
    {
        PlanName = planName;
        Content = content;
        CategoryEnum = categoryEnum;
        Email = email;
        CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, EventAggregator.Cest);
    }


    public enum TicketCategory
    {
        MISSING_PLAN,
        INCORRECT_DATA,
        SITE_ERROR,
        TECH_SUPPORT,
        OPINION,
        SUGGESTION,
        COOPERATION,
        OTHER
    }
}