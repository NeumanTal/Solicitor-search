using InfoTrack.Application.Interfaces;
using InfoTrack.Domain.Entities;

namespace InfoTrack.Application.Services;

public sealed class SolicitorSearch(
    ISolicitorScraper scraper,
    ISolicitorParser parser) : ISolicitorSearch
{
    public async Task<IEnumerable<Solicitor>> SearchByLocationsAsync(IEnumerable<string> locations, CancellationToken cancellationToken = default)
    {      
        var tasks = locations.Select(async location => 
            {
                var html = await scraper.GetHtmlByLocationAsync(location, cancellationToken);
                return parser.Parse(html, location);
            });

            var parsedResults = await Task.WhenAll(tasks);
            return parsedResults.SelectMany(x => x).ToList();
    }   
}