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

            //Check and valiate raw passengers list : will throw exception if an empty list or bad value
            rawPassengers.ValidateRawPassengers();

            // Generate passengers list
            var passengers = rawPassengers.GeneratePassengers();

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
        /// Generate passengers list based on raw passengers list
        /// </summary>
        /// <param name="rawPassengers"></param>
        /// <returns></returns>
        public static IEnumerable<Passenger> GeneratePassengers(this IEnumerable<RawPassenger> rawPassengers)
        {
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
        /// Validate all rawPassengers value will throw an exception if bad value provided
        /// </summary>
        /// <param name="rawPassengers"></param>
        /// <exception cref="Exception">When bad value found</exception>
        public static void ValidateRawPassengers(this IEnumerable<RawPassenger> rawPassengers)
        {
            var numberOfPassengers = rawPassengers.Count();
            if (numberOfPassengers == 0 || numberOfPassengers > Constants.MAX_PLANE_CAPACITY)
            {
                throw new Exception("Error an empty passenger list provided or over size max plane capacity !");
            }

            rawPassengers.ToList().ForEach(p => ValidatePassenger(p));

            static void ValidatePassenger(RawPassenger passenger)
            {
                var checkChildType = passenger.Age < Constants.PASSENGER_CHILD_AGE && !passenger.Type.Equals(PassengerTypeEnum.Enfant);
                var checkAdulteType = passenger.Age > Constants.PASSENGER_CHILD_AGE && !passenger.Type.Equals(PassengerTypeEnum.Adulte);
                //the child never sits alone !!
                var checkOversize = passenger.Places.Equals(Constants.PASSENGER_OVERSIZE_TYPE, StringComparison.Ordinal)
                    && !passenger.Type.Equals(PassengerTypeEnum.Adulte);

                if (checkChildType || checkAdulteType || checkOversize)
                {
                    var errorMessage = $"Erreur bad passenger provided with value : {passenger.ID,-5}{passenger.Type,-10}{passenger.Age,-5}{passenger.Famille,-5}{passenger.Places,-5}";
                    throw new Exception(errorMessage);
                }
            }
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
