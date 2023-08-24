using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using TuiFly.Turnover.Domain.Models;

namespace TuiFly.Turnover.Domain.Helpers
{
    /// <summary>
    /// A global helpers for read passengers csv file and Build raw list of 
    /// Potential passengers
    /// </summary>
    public static class RawPassengersHelpers
    {
        /// <summary>
        /// Read a raw passenger file and get list
        /// </summary>
        /// <param name="filePath">a simple csv file of raw passengers</param>
        /// <see cref="TuiFly.Turnover.Application.StaticFiles.passengers.csv"/>
        /// <returns></returns>
        public static IEnumerable<RawPassenger> ReadFile(string filePath)
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
                Console.WriteLine(ex.Message);
            }

            return Array.Empty<RawPassenger>();
        }
    }
}
