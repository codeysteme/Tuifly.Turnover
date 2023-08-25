using TuiFly.Turnover.Domain.Models;

namespace TuiFly.Turnover.Domain.Interfaces
{
    /// <summary>
    /// A global interface for manage all turnover service
    /// </summary>
    public interface ITurnoverManagerService
    {
        /// <summary>
        /// Display a disturb Plane with turnover
        /// </summary>
        /// <param name="passengerTickets">a passenger tickets</param>
        public void DisplayDistributePlaneWithTurnover(List<PassengerTicket> passengerTickets);

        /// <summary>
        /// distribute the passengers and families on the plane 
        /// We have default family for represents single passengers
        /// </summary>
        /// <param name="families">A list of families</param>
        public List<PassengerTicket> DistributePassengersAndFamiliesOnPlane(IEnumerable<Family> families);
    }
}
