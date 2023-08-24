namespace TuiFly.Turnover.Domain.Common
{
    /// <summary>
    /// A global constant class
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// the fixed price for Adulte passenger
        /// </summary>
        public const int ADULTE_PRICE = 250;

        /// <summary>
        /// the fixed price for Adulte passenger
        /// </summary
        public const int ENFANT_PRICE = 150;

        /// <summary>
        /// price for oversize passenger
        /// </summary>
        public const int OVERSIZE_PRICE = 500;

        /// <summary>
        /// passenger oversize type : two places
        /// </summary>
        public const string TWO_PLACES = "Oui";

        /// <summary>
        /// passenger one place
        /// </summary>
        public const string ONE_PLACE = "Non";

        /// <summary>
        /// Age to determine child passengers
        /// </summary>
        public const int PASSENGER_CHILD_AGE = 12;

        /// <summary>
        /// for all passengers who has not family : distinct by name : -
        /// </summary>
        public const string FAMILY_DEFAULT_NAME = "-";

        /// <summary>
        /// Max allowed number of Adult in familly
        /// </summary>
        public const int FAMILY_MAX_ADULT = 2;

        /// <summary>
        /// Max allowed number of Child in familly
        /// </summary>
        public const int FAMILY_MAX_CHILD = 3;

        /// <summary>
        /// the maximum number of passengers that can board the plane
        /// </summary>
        public const int MAX_PLANE_CAPACITY = 200;
    }
}
