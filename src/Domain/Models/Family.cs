using System.Collections.Generic;
using System.Linq;

namespace TuiFly.Turnover.Domain.Models
{
    /// <summary>
    /// Represent family class
    /// </summary>
    public class Family
    {
        #region properties

        /// <summary>
        /// The name for identify a family
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// All family members
        /// </summary>
        public IEnumerable<Passenger> Members { get; set; }

        /// <summary>
        /// Determine if family is valid : respect constraints 
        /// </summary>
        public bool Status { get; set; } = true;

        #endregion

        #region methods

        /// <summary>
        /// Invalidate status for family witch not respect constraints
        /// </summary>
        public void InvalidateStatus()
        {
            this.Status = false;
        }

        /// <summary>
        /// Calculate the price of a family ticket
        /// </summary>
        /// <returns></returns>
        public int CalculateTotalPrice()
        {
            return this.Members.Sum(p => p.Price);
        }

        #endregion
    }
}
