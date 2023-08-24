using System;
using TuiFly.Turnover.Domain.Services;

namespace TuiFly.Turnover.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            // Init turnover passenger manager service
            TurnoverManagerService.Init("StaticFiles/passengers.csv");


            Console.WriteLine("Hello World!");
        }
    }
}
