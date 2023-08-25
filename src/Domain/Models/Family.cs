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
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// All family members
        /// </summary>
        public List<Passenger> Members { get; set; } = new List<Passenger>();

        #endregion

        #region methods

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
