  using InfoTrack.Domain.Extensions;

  namespace InfoTrack.Domain.Entities;
  
  public class ReportResult
  {
    public IEnumerable<Solicitor>? Solicitors { get; set; }
    public int TotalSolicitors => Solicitors.IsNullOrEmpty() 
        ? 0 
        : Solicitors!.Count();
    public Solicitor? TopRatedSolicitor => Solicitors.IsNullOrEmpty()
        ? null
        : Solicitors!
            .Where(x => x.Rating.HasValue)
            .MaxBy(x => x.Rating);
    public decimal? AverageRating => Solicitors.IsNullOrEmpty()
        ? null
        : Solicitors!
            .Where(x => x.Rating.HasValue)
            .Average(x => x.Rating);  
}