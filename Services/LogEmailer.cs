using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UplLogEmail.Models.Data;
using UplLogEmail.Services.EmailService;

namespace UplLogEmail.Services;

public class LogEmailer(
    IUploadLogGetter uploadLogGetter,
    IEmailService emailService,
    IPdfGetter pdfGetter,
    ILogger<LogEmailer> logger
) : ILogEmailer
{
    private readonly IUploadLogGetter _uploadLogGetter = uploadLogGetter;
    private readonly IEmailService _emailService = emailService;
    private readonly IPdfGetter _pdfGetter = pdfGetter;
    private readonly ILogger<LogEmailer> _logger = logger;

    public async Task EmailLogs(FacilityData facility, DbContext context)
    {
        if (facility == null || context == null)
            throw new ArgumentNullException(facility == null ? nameof(facility) : nameof(context));

        var facilityName = facility.FacName ?? "Unknown Facility";

        try
        {
            var logs = await _uploadLogGetter.GetUploadedLogs(context, facility);

            if (logs == null || logs.Count == 0)
            {
                return;
            }

            var attachments = await GenerateAttachments(logs, facilityName);

            if (attachments.Count == 0)
                return;

            await SendEmailWithAttachments(facility, attachments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing logs for {FacilityName}", facilityName);
            throw;
        }
    }

    private async Task<Dictionary<string, byte[]>> GenerateAttachments(
        List<UploadLogData> logs,
        string facilityName
    )
    {
        var attachments = new Dictionary<string, byte[]>();
        var reportDate = DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy");

        try
        {
            var pdf = await _pdfGetter.GetUploadLogPdf(logs);
            if (pdf != null && pdf.Length > 0)
            {
                attachments.Add($"uploadlog_{reportDate}.pdf", pdf);
            }

            var phqLogs = logs.Where(l => l.NoteType == "PHQ9").ToList();
            var bimsLogs = logs.Where(l => l.NoteType == "BIMS").ToList();

            if (phqLogs.Count + bimsLogs.Count > 0)
            {
                var specializedNotes = new List<UploadLogData>();
                specializedNotes.AddRange(phqLogs);
                specializedNotes.AddRange(bimsLogs);

                var specializedPdf = await _pdfGetter.GetSummaryPdf(specializedNotes);
                if (specializedPdf != null && specializedPdf.Length > 0)
                {
                    attachments.Add($"phq9_bims_{reportDate}.pdf", specializedPdf);
                }
            }

            return attachments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDFs for {FacilityName}", facilityName);
            throw;
        }
    }

    private async Task SendEmailWithAttachments(
        FacilityData facility,
        Dictionary<string, byte[]> attachments
    )
    {
        var facilityName = facility.FacName ?? "Unknown Facility";

        try
        {
            _logger.LogInformation(
                "Sending email for {FacilityName} with {AttachmentCount} attachments",
                facilityName,
                attachments.Count
            );

            var response = await _emailService.SendEmail(facility, attachments);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email sent successfully for {FacilityName}", facilityName);
            }
            else
            {
                _logger.LogError(
                    "Failed to send email for {FacilityName}, Status: {StatusCode}",
                    facilityName,
                    (int)response.StatusCode
                );
                throw new ApplicationException(
                    $"Email service returned error status {response.StatusCode}"
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email for {FacilityName}", facilityName);
            throw;
        }
    }
}

public interface ILogEmailer
{
    Task EmailLogs(FacilityData facility, DbContext context);
}
