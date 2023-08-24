using TuiFly.Turnover.Domain.Common;
using TuiFly.Turnover.Domain.Models;

namespace TuiFly.Turnover.Domain.Services
{
    public static class TurnoverManagerService
    {
        /// <summary>
        /// Generate passengers list by a raw passenger file
        /// </summary>
        /// <param name="rawPassengerFile">path of static passenger file</param>
        /// <returns></returns>
        public static void Init(string rawPassengerFile)
        {
            // read passenger list from raw file
            //var rawPassengers = ReadRawPassengerFile(rawPassengerFile);

            //// Validate all rawPassengers filter and get valid passengers to sell tickets
            //var validRawPassengers = rawPassengers.FilterAndGetValidRawPassengers();

            //// Generate passengers list
            //var passengers = validRawPassengers.GenerateValidPassengersList();

            //// Generate passengers families
            //var families = passengers.GenerateAndValidateFamilies();

        }

        /// <summary>
        /// Generate families by passenger list with and 
        /// Check a valid family: respecting the constraints 2 adults && 3 children max
        /// </summary>
        /// <param name="passengers">A list of passenger</param>
        public static IEnumerable<Family> GenerateAndValidateFamilies(this IEnumerable<Passenger> passengers)
        {
            var passengerGroup = passengers.GroupBy(p => p.Family).Select(f => new Family { Name = f.Key, Members = f }).ToList();
            var familyGroup = passengerGroup.Where(f => !f.Name.Equals(Constants.FAMILY_DEFAULT_NAME)).ToList();

            var cleanFamilies = new List<Family>();

            foreach (var family in familyGroup)
            {
                var isMaxAdult = family.Members.Count(p => p.Type.Equals(PassengerTypeEnum.Adulte)) <= Constants.FAMILY_MAX_ADULT;
                var isMaxChild = family.Members.Count(p => p.Type.Equals(PassengerTypeEnum.Enfant)) <= Constants.FAMILY_MAX_CHILD;
                if (isMaxAdult && isMaxChild)
                {
                    cleanFamilies.Add(family);
                }
            }

            return cleanFamilies;
        }

        /// <summary>
        /// Generate valid passengers list : determine price of each pasenger
        /// </summary>
        /// <param name="rawPassengers"></param>
        /// <returns></returns>
        public static IEnumerable<Passenger> GenerateValidPassengersList(this IEnumerable<RawPassenger> rawPassengers)
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
