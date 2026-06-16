using InfoTrack.Application.Interfaces;

namespace InfoTrack.Infrastructure.Scrapers;

public sealed class MockSolicitorScraper : ISolicitorScraper
{
    public Task<string> GetHtmlByLocationAsync(string location, CancellationToken cancellationToken = default)
    {
        var html = File.ReadAllText(
            Path.Combine(
                AppContext.BaseDirectory,
                "Tests",
                "Conveyancing_solicitors_in_london.html"));

        return Task.FromResult(html);
    }
}