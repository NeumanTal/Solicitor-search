using InfoTrack.Application.Interfaces;

namespace InfoTrack.Infrastructure.Scrapers;

public sealed class SolicitorScraper(
    HttpClient httpClient) : ISolicitorScraper
{
    private const string BaseUrlSolicitorsDotCom = "https://www.solicitors.com";
    private const string ServiceType = "conveyancing";
    public async Task<string> GetHtmlByLocationAsync(string location, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = BuildLocationUrl(location);
            return await httpClient.GetStringAsync(
                url,
                cancellationToken);
        }
        catch(HttpRequestException ex)
        {
            Console.WriteLine($"[Scraper Error] Failed to fetch data for location '{location}'. Exception: {ex.Message}");
            return string.Empty;
        }
    }

    private static string BuildLocationUrl(string location)
    {
        var slug = location.Trim().ToLowerInvariant();
        return $"{BaseUrlSolicitorsDotCom}/{ServiceType}+{slug}.html";
    }
}