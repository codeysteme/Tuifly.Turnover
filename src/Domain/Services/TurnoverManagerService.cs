using TuiFly.Turnover.Domain.Common;
using TuiFly.Turnover.Domain.Interfaces;
using TuiFly.Turnover.Domain.Models;

namespace TuiFly.Turnover.Domain.Services
{
    public class TurnoverManagerService : ITurnoverManagerService
    {
        public static int IndexSeat = 1;

        /// <summary>
        /// Display a disturb Plane with turnover
        /// </summary>
        /// <param name="passengerTickets">a passenger tickets</param>
        public void DisplayDistributePlaneWithTurnover(List<PassengerTicket> passengerTickets)
        {
            var turnover = passengerTickets.Sum(x => x.Price);
            Console.WriteLine("****************************************************************************\n");
            Console.WriteLine($"{"",-20}{"   TuiFly Disturb Plane App",-20}\n");
            Console.WriteLine($"{"   Chiffre d'affaires total : ",-20}{turnover} Euro | {"Nombre de passagers :",-5} {passengerTickets.Count()} \n");
            Console.WriteLine("****************************************************************************\n");
            Console.WriteLine($"{"",-5}{"ID",-5}|{"TYPE",-10}|{"AGE",-10}|{"FAMILLE",-10}|{"PRIX",-10}|{"PLACES"}\n");

            int i = 1;
            foreach (var pt in passengerTickets)
            {
                var seats = pt.OverSize ? $"{pt.Seats[0],-5} and  {pt.Seats[1],-5}" : $"{pt.Seats[0]}";
                Console.WriteLine($"{"",-5}{i,-5}|{pt.Type,-10}|{pt.Age,-10}|{pt.Family,-10}|{pt.Price}{" Euro",-10}|{seats,-10}");
                i++;
            }
        }

        /// <summary>
        /// distribute the passengers and families on the plane 
        /// We have default family for represents single passengers
        /// </summary>
        /// <param name="families">A list of families</param>
        public List<PassengerTicket> DistributePassengersAndFamiliesOnPlane(IEnumerable<Family> families)
        {
            //start dispatch passengers
            // when adult is equal child : stable family

            var stablePassengers = new List<Passenger>();
            foreach (var family in families.Where(f => IsEqualFamily(f)).ToList())
            {
                var adults = family.Members.Where(p => p.Age > Constants.PASSENGER_CHILD_AGE).ToArray();
                var childs = family.Members.Where(p => p.Age < Constants.PASSENGER_CHILD_AGE).ToArray();

                for (int i = 0; i < adults.Length; i++)
                {
                    stablePassengers.Add(adults[i]);
                    stablePassengers.Add(childs[i]);
                }
            }

            //order not equal in mode parent => child
            var tempoPassengers = new List<Passenger>();
            foreach (var family in families.Where(f => IsNotEqualFamily(f)).ToArray())
            {
                var adults = family.Members.Where(p => p.Age > Constants.PASSENGER_CHILD_AGE).ToArray();
                var childs = family.Members.Where(p => p.Age < Constants.PASSENGER_CHILD_AGE).ToArray();

                var maxSize = adults.Length > childs.Length ? adults.Length : childs.Length;

                for (int i = 0; i < maxSize; i++)
                {
                    if (adults.Length > i)
                    {
                        tempoPassengers.Add(adults[i]);
                    }
                    if (childs.Length > i)
                    {
                        tempoPassengers.Add(childs[i]);
                    }
                }
            }

            //inject single passengers where two childs is sitting alone

            Family rawSinglePassengers = families.FirstOrDefault(f => f.Name.Equals(Constants.SINGLE_PASSENGER)) ?? new Family();
            var singlePassengers = rawSinglePassengers.Members.ToArray();

            var tempoNoEqual = tempoPassengers.ToArray();
            var isDuplicated = true;
            while (isDuplicated)
            {
                isDuplicated = false;
                for (int i = 0; i < tempoPassengers.Count(); i++)
                {
                    if ((i + 1) < tempoPassengers.Count() &&
                        (tempoNoEqual[i].Age < Constants.PASSENGER_CHILD_AGE && tempoNoEqual[i + 1].Age < Constants.PASSENGER_CHILD_AGE))
                    {
                        if (singlePassengers.Length > 0)
                        {
                            var adult = singlePassengers[0];
                            singlePassengers = singlePassengers.Where((e, i) => i != 0).ToArray();

                            var index = tempoPassengers.IndexOf(tempoNoEqual[i + 1]);
                            tempoPassengers.Insert(index, adult);

                            isDuplicated = true;
                        }
                        else
                        {
                            isDuplicated = false;
                        }
                        break;
                    }
                }
            }

            //check and remove childrens sitting alone
            var cleanPassengers = tempoPassengers;

            tempoNoEqual = tempoPassengers.ToArray();
            for (int i = 0; i < tempoNoEqual.Count(); i++)
            {
                if ((i + 1) < tempoNoEqual.Count() &&
                       (tempoNoEqual[i].Age < Constants.PASSENGER_CHILD_AGE && tempoNoEqual[i + 1].Age < Constants.PASSENGER_CHILD_AGE))
                {
                    cleanPassengers.RemoveAt(i);
                    if (cleanPassengers.Count() > (i + 1))
                    {
                        cleanPassengers.RemoveAt(i + 1);
                    }
                }
            }

            //Merge and build passenger tickets
            var passengerTickets = new List<PassengerTicket>();

            stablePassengers.ForEach(p => PushPassengerTicket(passengerTickets, p));
            cleanPassengers.ForEach(p => PushPassengerTicket(passengerTickets, p));
            singlePassengers.ToList().ForEach(p => PushPassengerTicket(passengerTickets, p));

            return passengerTickets;
        }

        /// <summary>
        /// Push a passenger
        /// </summary>
        /// <param name="passengerTickets"></param>
        /// <param name="passenger"></param>
        private static void PushPassengerTicket(List<PassengerTicket> passengerTickets, Passenger passenger)
        {
            try
            {
                if (passengerTickets.Count <= Constants.MAX_PLANE_CAPACITY)
                {
                    var passTicket = Passenger.ToPassengerTicket(passenger);
                    GenerateSeats(passTicket);
                    passengerTickets.Add(passTicket);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Generate passenger seats
        /// </summary>
        /// <param name="passengerTicket"></param>
        private static void GenerateSeats(PassengerTicket passengerTicket)
        {
            passengerTicket.Seats = passengerTicket.OverSize
            ? (new string[] { $"P_{IncrementByOne()}", $"P_{IncrementByOne()}" })
            : (new string[] { $"P_{IncrementByOne()}" });
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
