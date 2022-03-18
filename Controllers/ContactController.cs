using Microsoft.AspNetCore.Mvc;
using WATPlanCore.Aggregators;
using WATPlanCore.Data;
using WATPlanCore.ExternalServices;
using WATPlanCore.Models;

namespace WATPlanCore.Controllers;

[Route("api/contact")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly PlansDbContext _db;
    private readonly IMailService _mail;

    public ContactController(PlansDbContext dbContext, IMailService mail)
    {
        _db = dbContext;
        _mail = mail;
    }
    
    [HttpPost("ticket")]
    public async Task SendTicket([FromBody] Ticket ticket)
    {
        ticket.CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, EventAggregator.Cest);
        
        await _db.ContactTickets!.AddAsync(ticket);
        await _db.SaveChangesAsync();
        await _mail.TicketSent(ticket);
    }
}