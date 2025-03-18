using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using UplLogEmail.Models.Data;

namespace UplLogEmail.Services.EmailService;

public class EmailService(IConfiguration config) : IEmailService
{
    private readonly string? apiKey = config["SendGrid:ApiKey"];

    public async Task<Response> SendEmail(
        FacilityData facility,
        Dictionary<string, byte[]> attachments
    )
    {
        var emailClient = new SendGridClient(apiKey);
        var fromAddress = new EmailAddress("tsc@tscnotifications.com");
        var toAddresses = new List<EmailAddress> { new("mlemmer@thesupportivecare.com") };
        // var toAddresses = facility
        //     .Contacts!.Select(contact => new EmailAddress(contact.UplLogEmail))
        //     .ToList();
        var replyToAddress = new EmailAddress("it@thesupportivecare.com");
        var emailSubject =
            $"{facility.FacName} - PCC Upload Log {DateTime.Now.AddDays(-1):MM/dd/yyyy}";
        var textContent = "Attached, please find PCC upload log.";
        var htmlContent = $"<div>Attached, please find PCC upload log.</div>";
        var showAllRecipients = true;

        var emailMessage = MailHelper.CreateSingleEmailToMultipleRecipients(
            fromAddress,
            toAddresses,
            emailSubject,
            textContent,
            htmlContent,
            showAllRecipients
        );
        emailMessage.SetReplyTo(replyToAddress);

        foreach (var attachment in attachments)
        {
            var emailAttachment = new Attachment
            {
                Content = Convert.ToBase64String(attachment.Value),
                Type = "application/pdf",
                Filename = attachment.Key,
                Disposition = "attachment",
            };
            emailMessage.AddAttachment(emailAttachment);
        }
        // emailMessage.AddBcc(new EmailAddress("it@thesupportivecare.com"));

        var response = await emailClient.SendEmailAsync(emailMessage);

        return response;
    }
}

public interface IEmailService
{
    Task<Response> SendEmail(FacilityData facility, Dictionary<string, byte[]> pdfs);
}
