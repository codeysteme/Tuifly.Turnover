using Microsoft.Extensions.DependencyInjection;
using TuiFly.Turnover.Application.Extensions;
using TuiFly.Turnover.Domain.Interfaces;
using TuiFly.Turnover.Domain.Services;

namespace TuiFly.Turnover.Application
{
#pragma warning disable CS8602 
    class Program
    {
        static void Main(string[] args)
        {
            //setup D.I && config services
            var serviceProvider = ConfigureServices.GetServiceProvider();

            //Lauch process

            //Get and Build raw passenger from csv file
            var _rawPassengerService = serviceProvider.GetService<IRawPassengersService>();
            var rawPassengers = _rawPassengerService.GetRawPassengersList("StaticFiles/passengers.csv");

            if (rawPassengers != null && rawPassengers.Any())
            {
                // Validate all rawPassengers filter and get valid passengers to sell tickets And Build families
                var _passengerService = serviceProvider.GetService<IPassengerManagerService>();

                var validPassengerList = _passengerService.GeneratePassengersList(rawPassengers);
                var families = _passengerService.BuildFamiliesByPassengers(validPassengerList);

                //distribute the passengers and families on the plane 
                var passengerTickets = TurnoverManagerService.DistributePassengersAndFamiliesOnPlane(families);

                //Display disturb and turnover
                TurnoverManagerService.DisplayDistributePlaneWithTurnover(passengerTickets);
            }
        }
    }
}
