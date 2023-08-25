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
        /// distribute the passengers and families on the plane 
        /// We have default family for represents single passengers
        /// </summary>
        /// <param name="families">A list of families</param>
        public static void DistributePassengersAndFamiliesOnPlane(IEnumerable<Family> families)
        {

        }
    }
}
