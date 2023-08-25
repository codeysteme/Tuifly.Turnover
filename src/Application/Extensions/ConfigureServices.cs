using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TuiFly.Turnover.Domain.Interfaces;
using TuiFly.Turnover.Domain.Services;
using TuiFly.Turnover.Infrastructure.Services;

namespace TuiFly.Turnover.Application.Extensions
{
    /// <summary>
    /// Configure service and setup all D.I 
    /// </summary>
    public static class ConfigureServices
    {
        /// <summary>
        /// Build config Dependancy Injection and get serviceProvider
        /// </summary>
        /// <returns></returns>
        public static ServiceProvider GetServiceProvider()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(loginBuilder => loginBuilder.SetMinimumLevel(LogLevel.Debug).AddConsole())
                .AddSingleton<IRawPassengersService, RawPassengersService>()
                .AddSingleton<IPassengerManagerService, PassengerManagerService>()
                .AddSingleton<ITurnoverManagerService, TurnoverManagerService>()
                .BuildServiceProvider();

            serviceProvider
                .GetService<ILogger<RawPassengersService>>();

            return serviceProvider;
        }
    }
}
