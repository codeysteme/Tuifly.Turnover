using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TuiFly.Turnover.Domain.Common;
using TuiFly.Turnover.Domain.Models;

namespace TuiFly.Turnover.Domain.Services
{
    public class TurnoverManagerService
    {
        /// <summary>
        /// Generate passengers list by a raw passenger file
        /// </summary>
        /// <param name="rawPassengerFile">path of static passenger file</param>
        /// <returns></returns>
        public void init(string rawPassengerFile)
        {
            // read passenger list from raw file
            var records = ReadRawPassengerFile(rawPassengerFile);
            if (!records.Any())
            {
                return;
            }
            // Generate passengers list
            var passengers = GeneratePassengers(records);

            // Build passengers families
            var families = GenerateFamilies(passengers);

        }

        /// <summary>
        /// Generate families by passenger list
        /// </summary>
        /// <param name="passengers">A list of passenger</param>
        private static IEnumerable<Family> GenerateFamilies(IEnumerable<Passenger> passengers)
        {
            return passengers.GroupBy(p => p.Family)
                  .Select(f => new Family { Name = f.Key, Members = f.ToList() }).ToList();
        }

        /// <summary>
        /// Generate passengers list based on raw passengers list
        /// </summary>
        /// <param name="rawPassengers"></param>
        /// <returns></returns>
        private IEnumerable<Passenger> GeneratePassengers(IEnumerable<RawPassenger> rawPassengers)
        {
            return rawPassengers.Select(p => ToPassenger(p)).ToList();

            static Passenger ToPassenger(RawPassenger rawPassenger)
            {
                return new Passenger
                {
                    Age = rawPassenger.Age,
                    Family = rawPassenger.Famille,
                    Price = rawPassenger.Age < 12 ? Constants.ENFANT_PRICE : Constants.ADULTE_PRICE,
                    OverSize = rawPassenger.Places.Equals(Constants.PASSENGER_OVERSIZE_PRICE),
                    Type = rawPassenger.Type
                };
            }
        }

        /// <summary>
        /// Read passenger file and validate
        /// </summary>
        /// <param name="rawPassengerFile"></param>
        /// <returns></returns>
        private IEnumerable<RawPassenger> ReadRawPassengerFile(string rawPassengerFile)
        {
            try
            {
                using var reader = new StreamReader(rawPassengerFile);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                });

                return csv.GetRecords<RawPassenger>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Array.Empty<RawPassenger>();
        }
    }
}
