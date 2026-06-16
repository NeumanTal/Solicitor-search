using InfoTrack.Domain.Entities;
namespace InfoTrack.Application.Interfaces;

public interface ISolicitorParser
{
    IEnumerable<Solicitor> Parse(
        string html,
        string location);
}