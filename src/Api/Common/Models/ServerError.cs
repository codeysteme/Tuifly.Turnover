using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TuiFly.Turnover.Api.Common.Models
{
    /// <summary>
    /// Provide a custom server errorModel
    /// </summary>
    public class ServerError
    {
        /// <summary>
        /// Internal Server Error
        /// </summary>
        /// <example>Internal Server Error</example>
        public string Type { get; set; } = "Internal Server Error";

        /// <summary>
        /// Internal Server Error
        /// </summary>
        /// <example>Internal Server Error</example>
        public string Title { get; set; } = "Internal Server Error";

        /// <summary>
        /// The Error status
        /// </summary>
        /// <example>500</example>
        public int Status { get; set; } = StatusCodes.Status500InternalServerError;

        /// <summary>
        /// Internal Server Error
        /// </summary>
        /// <example>Internal Server Error</example>
        public string Detail { get; set; } = "Internal Server Error";

        /// <summary>
        /// Error Location
        /// </summary>
        /// <example>Error Location</example>
        public string Instance { get; set; }

        public ServerError()
        {
        }

        public ServerError(string instance)
        {
            Instance = instance;
        }
    }

    public static class ServerErrors
    {
        public static string ToJson(ServerError serverError)
        {
            return JsonConvert.SerializeObject(serverError);
        }
    }
}
