using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TuiFly.Turnover.Application.Extensions;
using TuiFly.Turnover.Domain.Interfaces;

namespace TuiFly.Turnover.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup D.I && config services
            var serviceProvider = ConfigureServices.GetServiceProvider();

            //Lauch process
            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger?.LogDebug("Starting application");

            //Get and Build raw passenger from csv file
            var _rawPassengerService = serviceProvider.GetService<IRawPassengersService>();
            var rawPassengers = _rawPassengerService.GetRawPassengersList("StaticFiles/passengers.csv");

            // Validate all rawPassengers filter and get valid passengers to sell tickets
            var _passengerService = serviceProvider.GetService<IPassengerManagerService>();

            var validPassengerList = _passengerService?.GeneratePassengersList(rawPassengers);



            Console.WriteLine("Hello World!");
        }
    }
}
