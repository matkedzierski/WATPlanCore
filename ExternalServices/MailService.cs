using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using WATPlanCore.Models;
using WATPlanCore.Models.Settings;

namespace WATPlanCore.ExternalServices;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger _logger;

    public MailService(IOptions<MailSettings> mailSettings, ILogger<MailService> logger)
    {
        _mailSettings = mailSettings.Value;
        _logger = logger;
    }

    public async Task TicketSent(Ticket ticket)
    {
        if (!string.IsNullOrEmpty(ticket.Email))
        {
            await SendEmail(PrepareTicketResponse(ticket));
        }

        await SendEmail(PrepareSelfNotification(ticket));
    }

    private MailMessage PrepareTicketResponse(Ticket ticket)
    {
        var email = PrepareEmptyEmail(true);
        email.Subject = $"[Support] Zgłoszenie nr {ticket.TicketId} z dnia {ticket.CreatedDate.ToShortDateString()}";
        email.To.Add(new MailAddress(ticket.Email));
        email.Body = BuildResponseEmailBody(ticket);
        return email;
    }

    private MailMessage PrepareSelfNotification(Ticket ticket)
    {
        var email = PrepareEmptyEmail(true);
        email.Subject = $"[{ticket.CategoryName}] Zgłoszenie nr {ticket.TicketId} z dnia {ticket.CreatedDate.ToShortDateString()}";
        email.To.Add(new MailAddress(_mailSettings.Mail));
        email.Body = BuildNotificationEmailBody(ticket);
        return email;
    }

    private MailMessage PrepareEmptyEmail(bool html)
    {
        var email = new MailMessage();
        email.Sender = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
        email.From = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
        email.IsBodyHtml = html;
        return email;
    }

    private async Task SendEmail(MailMessage email)
    {
        _logger.LogDebug("Sending email '{subject}' to '{addressee}'", email.Subject, email.To.First().Address);
        using var smtp = new SmtpClient(_mailSettings.Host, _mailSettings.Port);
        smtp.EnableSsl = true;
        smtp.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendMailAsync(email);
        smtp.Dispose();
    }

    private static string BuildNotificationEmailBody(Ticket ticket)
    {
        var html = File.ReadAllText("HTMLTemplates/ticket_notification.html");
        return Fill(html, ticket).Replace(WP_TICKET_R_SUBJECT, $"[Support] RE: Zgłoszenie nr {ticket.TicketId} z dnia {ticket.CreatedDate.ToShortDateString()}");
    }

    private static string BuildResponseEmailBody(Ticket ticket)
    {
        var html = File.ReadAllText("HTMLTemplates/ticket_response.html");
        return Fill(html, ticket);
    }

    private static string Fill(string html, Ticket ticket)
    {
        return html
            .Replace(WP_TICKET_NO, ticket.TicketId)
            .Replace(WP_TICKET_DATE, ticket.CreatedDate.ToShortDateString())
            .Replace(WP_TICKET_TIME, ticket.CreatedDate.ToShortTimeString())
            .Replace(WP_TICKET_CATEGORY, ticket.CategoryName)
            .Replace(WP_TICKET_PLANNAME, ticket.PlanName)
            .Replace(WP_TICKET_CONTENT, ticket.Content)
            .Replace(WP_TICKET_SENDER_OR_EMPTY, string.IsNullOrEmpty(ticket.Sender) ? "" : ", " + ticket.Sender)
            .Replace(WP_TICKET_SENDER, string.IsNullOrEmpty(ticket.Sender) ? "" : ticket.Sender);
    }

    private const string WP_TICKET_NO = "{WP_TICKET_NO}";
    private const string WP_TICKET_DATE = "{WP_TICKET_DATE}";
    private const string WP_TICKET_TIME = "{WP_TICKET_TIME}";
    private const string WP_TICKET_CATEGORY = "{WP_TICKET_CATEGORY}";
    private const string WP_TICKET_PLANNAME = "{WP_TICKET_PLANNAME}";
    private const string WP_TICKET_CONTENT = "{WP_TICKET_CONTENT}";
    private const string WP_TICKET_SENDER = "{WP_TICKET_SENDER}";
    private const string WP_TICKET_SENDER_OR_EMPTY = "{WP_TICKET_SENDER_OR_EMPTY}";
    private const string WP_TICKET_R_SUBJECT = "{WP_TICKET_R_SUBJECT}";
}