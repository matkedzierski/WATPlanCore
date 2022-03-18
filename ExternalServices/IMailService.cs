using WATPlanCore.Models;

namespace WATPlanCore.ExternalServices;

public interface IMailService
{
    Task TicketSent(Ticket ticket);
}