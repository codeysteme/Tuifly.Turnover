using System;
using TuiFly.Turnover.Domain.Services;

namespace TuiFly.Turnover.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            // Init turnover passenger manager service
            var turnoverService = new TurnoverManagerService();

            // Generate passenger file by a raw data file
            turnoverService.init("StaticFiles/passengers.csv");

            if (true)
                return;

            //var listPassengers = 

            //turnoverManagerService.GeneratePassengersAndFamily();


            Console.WriteLine("Hello World!");
        }
    }
}
