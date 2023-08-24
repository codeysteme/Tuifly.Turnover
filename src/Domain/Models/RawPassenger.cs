using TuiFly.Turnover.Domain.Common;

namespace TuiFly.Turnover.Domain.Models
{
    /// <summary>
    /// Model represent a raw passenger from file
    /// </summary>
    public class RawPassenger
    {
        /// <summary>
        /// A passenger Id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Passenger type : Adulte | Enfant
        /// </summary>
        public PassengerTypeEnum Type { get; set; }

        /// <summary>
        /// Age of passenger
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Family name
        /// </summary>
        public string Famille { get; set; } = string.Empty;

        /// <summary>
        /// Passenger places number : check
        /// </summary>
        public string Places { get; set; } = string.Empty;
    }
}
