using TuiFly.Turnover.Domain.Common;

namespace TuiFly.Turnover.Domain.Models
{
    /// <summary>
    /// the model for represent passenger
    /// </summary>
    public class Passenger
    {
        /// <summary>
        /// the type of passenger : use for distinct user
        /// </summary>
        /// <see cref="TuiFly.Turnover.Domain.Common.PassengerTypeEnum"/>
        /// <example>Adulte</example>
        public PassengerTypeEnum Type { get; set; }

        /// <summary>
        /// The passenger age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// the family id of passenger
        /// </summary>
        /// <example>A</example>
        public string Family { get; set; }

        /// <summary>
        /// passenger flight price
        /// </summary>
        /// <example>250</example>
        public int Price { get; set; }

        /// <summary>
        /// Determine if user need two places
        /// </summary>
        /// <example>true</example>
        public bool OverSize { get; set; }
    }
}
