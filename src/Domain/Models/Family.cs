using System.Collections.Generic;

namespace TuiFly.Turnover.Domain.Models
{
    /// <summary>
    /// Represent family class
    /// </summary>
    public class Family
    {
        /// <summary>
        /// The name for identify a family
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// All family members
        /// </summary>
        public IEnumerable<Passenger> Members { get; set; }
    }
}
