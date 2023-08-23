using Microsoft.Extensions.DependencyInjection;

namespace TuiFly.Turnover.Api.Extensions
{
    /// <summary>
    /// Configrure and Inject all D.I of app
    /// </summary>
    public static class ConfigureServices
    {
        /// <summary>
        /// config D.I
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppServices(this IServiceCollection service)
        {
            return service;
        }
    }
}
