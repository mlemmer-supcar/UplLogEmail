using System.Text.Json;
using Microsoft.Extensions.Logging;
using UplLogEmail.Constants;
using UplLogEmail.Models.Data;

namespace UplLogEmail.Services;

public class PdfGetter(IHttpClientFactory httpClientFactory, ILogger<PdfGetter> logger) : IPdfGetter
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly Uri _baseUri = new(Urls.PDF_GEN_URL);
    private readonly ILogger<PdfGetter> _logger = logger;

    public async Task<byte[]> GetUploadLogPdf(List<UploadLogData> logs)
    {
        _logger.LogInformation("Getting upload log PDF");

        string requestUri = "api/pdf-gen/summary/uploadlog";

        using var httpClient = CreateHttpClient();

        var content = new StringContent(
            JsonSerializer.Serialize(logs),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        try
        {
            HttpResponseMessage response = await httpClient.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Failed to generate upload log PDF. URL: {RequestUri}, Status: {StatusCode}, ReasonPhrase: {ReasonPhrase}",
                    requestUri,
                    (int)response.StatusCode,
                    response.ReasonPhrase
                );
                return [];
            }

            byte[] pdfData = await response.Content.ReadAsByteArrayAsync();
            _logger.LogInformation(
                "Successfully generated upload log PDF. Size: {Size} bytes",
                pdfData.Length
            );
            return pdfData;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "HTTP request error while generating upload log PDF. URL: {RequestUri}",
                requestUri
            );
            return [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating upload log PDF");
            return [];
        }
    }

    public async Task<byte[]> GetSummaryPdf(List<UploadLogData> logs)
    {
        _logger.LogInformation("Getting summary PDF. Notes Count: {Count}", logs.Count);

        string requestUri = $"api/pdf-gen/summary/uploadLogBimsPhq/";

        using var httpClient = CreateHttpClient();
        var content = new StringContent(
            JsonSerializer.Serialize(logs),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        try
        {
            HttpResponseMessage response = await httpClient.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Failed to generate summary PDF. URL: {RequestUri}, Status: {StatusCode}, ReasonPhrase: {ReasonPhrase}",
                    requestUri,
                    (int)response.StatusCode,
                    response.ReasonPhrase
                );
                return Array.Empty<byte>();
            }

            byte[] pdfData = await response.Content.ReadAsByteArrayAsync();
            _logger.LogInformation(
                "Successfully generated summary PDF. Size: {Size} bytes",
                pdfData.Length
            );
            return pdfData;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "HTTP request error while generating summary PDF. URL: {RequestUri}",
                requestUri
            );
            return Array.Empty<byte>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating summary PDF");
            return Array.Empty<byte>();
        }
    }

    private HttpClient CreateHttpClient()
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = _baseUri;
        return httpClient;
    }
}

public interface IPdfGetter
{
    Task<byte[]> GetUploadLogPdf(List<UploadLogData> notes);
    Task<byte[]> GetSummaryPdf(List<UploadLogData> notes);
}
