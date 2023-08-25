using TuiFly.Turnover.Domain.Models;

namespace TuiFly.Turnover.Domain.Interfaces
{
    /// <summary>
    /// Contracts for manage all raw passengers file and generate list
    /// </summary>
    public interface IRawPassengersService
    {
        /// <summary>
        /// Read, filter and get list of valid rawPassengers who respect all constraints
        /// </summary>
        /// <param name="filePath">a simple csv file of raw passengers</param>
        /// <see cref="Turnover.Application.StaticFiles.passengers.csv"/>
        /// <remarks>filePath must be a vlid csv file</remarks>
        /// <returns></returns>
        public List<RawPassenger> GetRawPassengersList(string filePath);

        /// <summary>
        /// Read a raw passenger file and get list
        /// </summary>
        /// <param name="filePath">a simple csv file of raw passengers</param>
        /// <see cref="Turnover.Application.StaticFiles.passengers.csv"/>
        /// <returns></returns>
        public IEnumerable<RawPassenger> ReadFile(string filePath);
    }
}
