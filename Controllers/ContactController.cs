using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WATPlanCore.Data;
using WATPlanCore.ExternalServices;
using WATPlanCore.ExternalServices.Aggregators;
using WATPlanCore.Models;

namespace WATPlanCore.Controllers;

/// <summary>
/// Umożliwia przesyłanie zgłoszeń do supportu aplikacji.
/// </summary>
[Produces("application/json")]
[Route("api/contact")]
[ApiController]
[EnableCors("watplan")]
public class ContactController : ControllerBase
{
    private readonly PlansDbContext _db;
    private readonly IMailService _mail;

    public ContactController(PlansDbContext dbContext, IMailService mail)
    {
        _db = dbContext;
        _mail = mail;
    }
    
    /// <summary>
    /// Przesyła nowe zgłoszenie do supportu.
    /// </summary>
    /// <param name="ticket">Model zgłoszenia (Ticket) z jego szczegółami.</param>
    [HttpPost("ticket")]
    public async Task SendTicket([FromBody] Ticket ticket)
    {
        ticket.CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, EventAggregator.Cest);
        
        await _db.ContactTickets!.AddAsync(ticket);
        await _db.SaveChangesAsync();
        await _mail.TicketSent(ticket);
    }
}