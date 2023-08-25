using TuiFly.Turnover.Domain.Common;
using TuiFly.Turnover.Domain.Interfaces;
using TuiFly.Turnover.Domain.Models;

namespace TuiFly.Turnover.Domain.Services
{
    /// <summary>
    /// A service for managing passenger list and families creation
    /// </summary>
    public class PassengerManagerService : IPassengerManagerService
    {
        /// <summary>
        /// Generate valid passengers list and determine price of each pasenger
        /// </summary>
        /// <param name="rawPassengers">A raw passenger file</param>
        /// <returns></returns>
        /// <see cref="Interfaces.IRawPassengersService"/>
        public IEnumerable<Passenger> GeneratePassengersList(IEnumerable<RawPassenger> rawPassengers)
        {
            if (!rawPassengers.Any())
                return Array.Empty<Passenger>();

            return rawPassengers.Select(p => ToPassenger(p)).OrderBy(p => p.Family).ToList();

            static Passenger ToPassenger(RawPassenger rawPassenger)
            {
                var isOversize = rawPassenger.Places.Equals(Constants.TWO_PLACES, StringComparison.Ordinal);
                var price = isOversize ? Constants.OVERSIZE_PRICE : rawPassenger.Age < 12 ? Constants.ENFANT_PRICE : Constants.ADULTE_PRICE;

                return new Passenger
                {
                    Age = rawPassenger.Age,
                    Family = rawPassenger.Famille,
                    Type = rawPassenger.Type,
                    OverSize = isOversize,
                    Price = price
                };
            }
        }

        /// <summary>
        /// Generate families by passenger list with and 
        /// Check a valid family: respecting the constraints 2 adults && 3 children max
        /// </summary>
        /// <param name="passengers">A list of passenger</param>
        public IEnumerable<Family> BuildFamiliesByPassengers(IEnumerable<Passenger> passengers)
        {
            var families = passengers.GroupBy(p => p.Family).Select(f => new Family { Name = f.Key, Members = f }).ToList();
            var notSingleFamilies = families.Where(f => !f.Name.Equals(Constants.SINGLE_PASSENGER)).ToList();

            foreach (var family in notSingleFamilies)
            {
                if ((family.Members.Count(p => p.Type.Equals(PassengerTypeEnum.Adulte)) > Constants.FAMILY_MAX_ADULT)
                    || (family.Members.Count(p => p.Type.Equals(PassengerTypeEnum.Enfant)) > Constants.FAMILY_MAX_CHILD))
                {
                    families.Remove(family);
                }
            }

            return families;
        }
    }
}
