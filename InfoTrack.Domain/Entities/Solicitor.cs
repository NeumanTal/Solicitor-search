namespace InfoTrack.Domain.Entities;

public sealed class Solicitor
{
    public string Name { get ; set; } = string.Empty;
    public string PhoneNumber { get ; set; } = string.Empty;
    public string Address { get ; set; } = string.Empty;
    public string Description { get ; set; } = string.Empty;
    public decimal? Rating { get ; set; }
    public int NoOfReviews { get ; set; }
    public string Location { get ; set; } = string.Empty;
    public string Website { get ; set; } = string.Empty;
    public string ProfileUrl { get ; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}