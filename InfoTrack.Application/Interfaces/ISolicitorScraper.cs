namespace InfoTrack.Application.Interfaces;

public interface ISolicitorScraper
{
    Task<string> GetHtmlByLocationAsync(string location, CancellationToken cancellationToken = default);
}