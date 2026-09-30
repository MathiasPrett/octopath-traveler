namespace Octopath_Traveler.Models;

public interface IUnitVisitor<T>
{
    T VisitTraveler(Traveler traveler);
    T VisitBeast(Beast beast);
}
