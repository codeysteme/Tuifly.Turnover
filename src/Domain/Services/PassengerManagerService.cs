using TuiFly.Turnover.Domain.Common;
using TuiFly.Turnover.Domain.Models;

namespace TuiFly.Turnover.Domain.Services
{
    /// <summary>
    /// the passenger manager service
    /// </summary>
    public class PassengerManagerService
    {
        /// <summary>
        /// Generate valid passengers list and determine price of each pasenger
        /// </summary>
        /// <param name="rawPassengers">A raw passenger file</param>
        /// <returns></returns>
        /// <see cref="Interfaces.IRawPassengersService"/>
        public static IEnumerable<Passenger> GeneratePassengersList(IEnumerable<RawPassenger> rawPassengers)
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
    }
}
