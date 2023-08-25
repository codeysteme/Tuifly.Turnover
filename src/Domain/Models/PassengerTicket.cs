namespace TuiFly.Turnover.Domain.Models
{
    /// <summary>
    /// A model for assign passenger place in plane
    /// </summary>
    public class PassengerTicket : Passenger
    {
        /// <summary>
        /// The passenger seats 
        /// PS : An Adulte can have one or two seats
        /// </summary>
        public string[] Seats { get; set; } = Array.Empty<string>();
    }
}
