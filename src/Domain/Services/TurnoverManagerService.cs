using TuiFly.Turnover.Domain.Common;
using TuiFly.Turnover.Domain.Models;

namespace TuiFly.Turnover.Domain.Services
{
    public static class TurnoverManagerService
    {
        public static int IndexSeat = 1;

        /// <summary>
        /// Display a disturb Plane with turnover
        /// </summary>
        /// <param name="passengerTickets">a passenger tickets</param>
        public static void DisplayDistributePlaneWithTurnover(List<PassengerTicket> passengerTickets)
        {
            ///ID;Type;Age;Famille;Places
            //1; Adulte; 35; A; Non

            var turnover = passengerTickets.Sum(x => x.Price);
            Console.WriteLine("****************************************************************************\n");
            Console.WriteLine($"{"",-20}{"   TuiFly Disturb Plane App",-20}\n");
            Console.WriteLine($"{"   Chiffre d'affaires total : ",-20}{turnover} Euro | {"Nombre de passagers :",-5} {passengerTickets.Count()} \n");
            Console.WriteLine("****************************************************************************\n");
            Console.WriteLine($"{"",-5}{"ID",-5}|{"TYPE",-10}|{"AGE",-10}|{"FAMILLE",-10}|{"PLACES"}\n");

            int i = 1;
            foreach (var pt in passengerTickets)
            {
                var seats = pt.OverSize ? $"{pt.Seats[0],-5} and  {pt.Seats[1],-5}" : $"{pt.Seats[0]}";
                Console.WriteLine($"{"",-5}{i,-5}|{pt.Type,-10}|{pt.Age,-10}|{pt.Family,-10}|{seats,-10}");
                i++;
            }
        }

        /// <summary>
        /// distribute the passengers and families on the plane 
        /// We have default family for represents single passengers
        /// </summary>
        /// <param name="families">A list of families</param>
        public static List<PassengerTicket> DistributePassengersAndFamiliesOnPlane(IEnumerable<Family> families)
        {
            // filter family : equal where number of children equal parents | none equal | and singles
            var equalFamilies = families.Where(f => IsEqualFamily(f)).ToList();
            var notEqualFamilies = families.Where(f => IsNotEqualFamily(f)).ToList();
            var singlePassengers = families.FirstOrDefault(f => f.Name.Equals(Constants.SINGLE_PASSENGER));

            //start dispatch passengers
            var passengerTickets = new List<PassengerTicket>();


            foreach (var family in equalFamilies)
            {
                foreach (var passenger in family.Members)
                {
                    var passTicket = Passenger.ToPassengerTicket(passenger);
                    GenerateSeats(passTicket);
                    passengerTickets.Add(passTicket);
                }
            }

            return passengerTickets;
        }

        /// <summary>
        /// Generate passenger seats
        /// </summary>
        /// <param name="passengerTicket"></param>
        private static void GenerateSeats(PassengerTicket passengerTicket)
        {
            if (passengerTicket.OverSize)
            {
                passengerTicket.Seats = new string[] { $"P_{IncrementByOne()}", $"P_{IncrementByOne()}" };
            }
            else
            {
                passengerTicket.Seats = new string[] { $"P_{IncrementByOne()}" };
            }
        }

        public static int IncrementByOne() => IndexSeat++;

        /// <summary>
        /// Get an equal families
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        private static bool IsEqualFamily(Family family)
        {
            return !family.Name.Equals(Constants.SINGLE_PASSENGER)
                && family.Members.Count(p => p.Type.Equals(PassengerTypeEnum.Adulte)) == family.Members.Count(p => p.Type.Equals(PassengerTypeEnum.Enfant));
        }

        /// <summary>
        /// Get not equal families
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        private static bool IsNotEqualFamily(Family family)
        {
            return !family.Name.Equals(Constants.SINGLE_PASSENGER)
                && family.Members.Count(p => p.Type.Equals(PassengerTypeEnum.Adulte)) != family.Members.Count(p => p.Type.Equals(PassengerTypeEnum.Enfant));
        }
    }
}
