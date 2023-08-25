using TuiFly.Turnover.Domain.Models;

namespace TuiFly.Turnover.Domain.Interfaces
{
    /// <summary>
    /// A contract for managing passenger list and families creation
    /// </summary>
    public interface IPassengerManagerService
    {
        /// <summary>
        /// Generate valid passengers list and determine price of each pasenger
        /// </summary>
        /// <param name="rawPassengers">A raw passenger file</param>
        /// <returns></returns>
        /// <see cref="Interfaces.IRawPassengersService"/>
        public IEnumerable<Passenger> GeneratePassengersList(IEnumerable<RawPassenger> rawPassengers);

        /// <summary>
        /// Generate families by passenger list with and 
        /// Check a valid family: respecting the constraints 2 adults && 3 children max
        /// </summary>
        /// <param name="passengers">A list of passenger</param>
        public IEnumerable<Family> BuildFamiliesByPassengers(IEnumerable<Passenger> passengers);
    }
}
