using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using TuiFly.Turnover.Domain.Common;
using TuiFly.Turnover.Domain.Interfaces;
using TuiFly.Turnover.Domain.Models;

namespace TuiFly.Turnover.Infrastructure.Services
{
    /// <summary>
    /// A service for mamange read passengers csv file and Build raw list of 
    /// Potential passengers
    /// </summary>
    public class RawPassengersService : IRawPassengersService
    {
        private readonly ILogger _logger;

        /// <summary>
        /// the ctor
        /// </summary>
        /// <param name="logger"></param>
        public RawPassengersService(ILogger<RawPassengersService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Read, filter and get list of valid rawPassengers who respect all constraints
        /// </summary>
        /// <param name="filePath">a simple csv file of raw passengers</param>
        /// <see cref="Turnover.Application.StaticFiles.passengers.csv"/>
        /// <remarks>filePath must be a vlid csv file</remarks>
        /// <returns></returns>
        public List<RawPassenger> GetRawPassengersList(string filePath) => FilterAndGetRawPassengers(ReadFile(filePath));

        /// <summary>
        /// Read a raw passenger file and get list
        /// </summary>
        /// <param name="filePath">a simple csv file of raw passengers</param>
        /// <see cref="Turnover.Application.StaticFiles.passengers.csv"/>
        /// <returns></returns>
        public IEnumerable<RawPassenger> ReadFile(string filePath)
        {
            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                });

                return csv.GetRecords<RawPassenger>().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error when thry to load file path : {}. With message : {}", filePath, ex.Message);
            }

            return Array.Empty<RawPassenger>();
        }

        /// <summary>
        /// Validate all rawPassengers, filter and get valid passengers to sell tickets
        /// </summary>
        /// <param name="rawPassengers"></param>
        public static List<RawPassenger> FilterAndGetRawPassengers(IEnumerable<RawPassenger> rawPassengers)
        {
            var validRawPassengers = new List<RawPassenger>();
            foreach (var passenger in rawPassengers)
            {
                if (passenger.Type.Equals(PassengerTypeEnum.Adulte) && passenger.Age >= Constants.PASSENGER_CHILD_AGE)
                {
                    validRawPassengers.Add(passenger);
                    continue;
                }

                //Validate child: must have at least one parent : same family with adult and not << - >>
                if (passenger.Type.Equals(PassengerTypeEnum.Enfant))
                {
                    var isValidChild = passenger.Age < Constants.PASSENGER_CHILD_AGE
                        && !passenger.Famille.Equals(Constants.SINGLE_PASSENGER)
                        && !passenger.Places.Equals(Constants.TWO_PLACES, StringComparison.Ordinal)
                        && rawPassengers.Any(p => p.Type.Equals(PassengerTypeEnum.Adulte) && p.Famille.Equals(passenger.Famille));
                    if (isValidChild)
                    {
                        validRawPassengers.Add(passenger);
                        continue;
                    }
                }
            }

            return validRawPassengers;
        }
    }
}
