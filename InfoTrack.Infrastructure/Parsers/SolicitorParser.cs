using InfoTrack.Application.Interfaces;
using InfoTrack.Domain.Entities;
using System.Net;
using System.Text.RegularExpressions;

namespace InfoTrack.Infrastructure.Parsers;

public sealed class SolicitorParser : ISolicitorParser
{
    public IEnumerable<Solicitor> Parse(string html, string location)
    {
        var results = new List<Solicitor>();
        var blocks = html.Split(
            "<div class=\"result-item\">",
            StringSplitOptions.RemoveEmptyEntries);

        foreach (var block in blocks)
        {
            var name = ExtractName(block);

            if (string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            results.Add(new Solicitor
            {
                Name = name,
                PhoneNumber = ExtractPhone(block),
                Address = ExtractAddress(block),
                Website = ExtractWebsite(block),
                Description = ExtractDescription(block),
                Rating = ExtractRating(block),
                NoOfReviews = ExtractReviewCount(block),
                Location = location,
                ProfileUrl = ExtractProfileUrl(block)
            });
        }

        return results;
    }

    private static string ExtractName(string html)
    {
        var match = Regex.Match(
            html,
            @"<span class=""h2"">\s*([^<]+)",
            RegexOptions.Singleline);

        return match.Success
            ? Clean(match.Groups[1].Value)
            : string.Empty;
    }

    private static string ExtractPhone(string html)
    {
        var match = Regex.Match(
            html,
            @"href=""tel:(.*?)""");

        return match.Success
            ? Clean(match.Groups[1].Value)
            : string.Empty;
    }

    private static string ExtractAddress(string html)
    {
        var match = Regex.Match(
            html,
            @"<address>(.*?)</address>",
            RegexOptions.Singleline);

        return match.Success
            ? Clean(match.Groups[1].Value)
            : string.Empty;
    }

    private static string ExtractDescription(string html)
    {
        var match = Regex.Match(
            html,
            @"</address>\s*</a>\s*<p>(.*?)</p>",
            RegexOptions.Singleline);

        return match.Success
            ? Clean(match.Groups[1].Value)
            : string.Empty;
    }

    private static string ExtractWebsite(string html)
    {
        var match = Regex.Match(
            html,
            @"target=""_blank""\s+href=""(.*?)""",
            RegexOptions.Singleline);

        return match.Success
            ? Clean(match.Groups[1].Value)
            : string.Empty;
    }

    private static string ExtractProfileUrl(string html)
    {
        var match = Regex.Match(
            html,
            @"<a href=""(/.*?\.html)"" class=""link-map""",
            RegexOptions.Singleline);

        if (!match.Success)
        {
            return string.Empty;
        }

        return $"https://www.solicitors.com{match.Groups[1].Value}";
    }

    private static decimal? ExtractRating(string html)
    {
         var match = Regex.Match(
        html,
        @"<span class=""rev-results"">(.*?)</span>",
        RegexOptions.Singleline);

        if (!match.Success)
        {
            return null;
        }

        var ratingHtml = match.Groups[1].Value;

        var fullStars =
            Regex.Matches(
                ratingHtml,
                @"star-full").Count;

        var halfStars =
            Regex.Matches(
                ratingHtml,
                @"star-half").Count;

        return fullStars + (halfStars * 0.5m);
    }

    private static int ExtractReviewCount(string html)
    {
        var match = Regex.Match(
        html,
        @"<span class=""rev-results"">.*?\((\d+)\)",
        RegexOptions.Singleline);

        return match.Success
            ? int.Parse(match.Groups[1].Value)
            : 0;
    }

    private static string Clean(string value)
    {
        return WebUtility.HtmlDecode(value)
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace("\t", "")
            .Replace("<br>", " ")
            .Trim();
    }    
}