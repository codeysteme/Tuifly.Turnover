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
            var rawPassengers = ReadRawPassengerFile(rawPassengerFile);

            // Validate all rawPassengers filter and get valid passengers to sell tickets
            var validRawPassengers = rawPassengers.FilterAndGetValidRawPassengers();

            // Generate passengers list
            var passengers = validRawPassengers.GenerateValidPassengersList();

            // Generate passengers families
            var families = passengers.GenerateAndValidateFamilies();

        }

        /// <summary>
        /// Generate families by passenger list with and 
        /// Check a valid family: respecting the constraints 2 adults && 3 children max
        /// </summary>
        /// <param name="passengers">A list of passenger</param>
        public static IEnumerable<Family> GenerateAndValidateFamilies(this IEnumerable<Passenger> passengers)
        {
            var families = passengers.GroupBy(p => p.Family).Select(f => new Family { Name = f.Key, Members = f, Status = true }).ToList();
            var familyGroup = families.Where(f => !f.Name.Equals(Constants.FAMILY_DEFAULT_NAME)).ToArray();

            for (int i = 0; i < families.Count; i++)
            {
                if (families[i].Members.Count(p => p.Type.Equals(PassengerTypeEnum.Adulte)) > Constants.FAMILY_MAX_ADULT
                     || families[i].Members.Count(p => p.Type.Equals(PassengerTypeEnum.Enfant)) > Constants.FAMILY_MAX_CHILD)
                {
                    families[i].InvalidateStatus();
                }
            }

            return families.ToList();
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
                var isOversize = rawPassenger.Places.Equals(Constants.PASSENGER_OVERSIZE_TYPE, StringComparison.Ordinal);
                var price = isOversize ? Constants.OVERSIZE_PRICE : rawPassenger.Age < 12 ? Constants.ENFANT_PRICE : Constants.ADULTE_PRICE;

                return new Passenger
                {
                    Age = rawPassenger.Age,
                    Family = rawPassenger.Famille,
                    OverSize = isOversize,
                    Type = rawPassenger.Type,
                    Price = price
                };
            }
        }

        /// <summary>
        /// Validate all rawPassengers, filter and get valid passengers to sell tickets
        /// </summary>
        /// <param name="rawPassengers"></param>
        public static List<RawPassenger> FilterAndGetValidRawPassengers(this IEnumerable<RawPassenger> rawPassengers)
        {
            var validRawPassengers = new List<RawPassenger>();
            foreach (var passenger in rawPassengers)
            {
                //Validate type of passenger ADULT or Child
                var isNotValidType = passenger.Age < Constants.PASSENGER_CHILD_AGE && !passenger.Type.Equals(PassengerTypeEnum.Enfant);

                //Child passenger must have at least one parent : same family with adult and not << - >>
                var isChildHasNoParent = passenger.Age < Constants.PASSENGER_CHILD_AGE && passenger.Famille.Equals(Constants.FAMILY_DEFAULT_NAME);

                //Only adult can have two sits
                var isNotValidOverSize = passenger.Places.Equals(Constants.PASSENGER_OVERSIZE_TYPE, StringComparison.Ordinal)
                    && !passenger.Type.Equals(PassengerTypeEnum.Adulte);

                if (!isNotValidType && !isChildHasNoParent && !isNotValidOverSize)
                {
                    validRawPassengers.Add(passenger);
                }
            }

            return validRawPassengers;
        }

        /// <summary>
        /// Read passenger file and validate
        /// </summary>
        /// <param name="rawPassengerFile"></param>
        /// <returns></returns>
        public static IEnumerable<RawPassenger> ReadRawPassengerFile(string rawPassengerFile)
        {
            try
            {
                using var reader = new StreamReader(rawPassengerFile);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                });

                return csv.GetRecords<RawPassenger>().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Array.Empty<RawPassenger>();
        }
    }
}
