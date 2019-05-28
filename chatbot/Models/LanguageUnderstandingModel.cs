using chatbot.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace chatbot.Models
{
    public class LanguageUnderstandingModel
    {
        private static readonly string AppKey = ConfigurationManager.AppSetting["Configurations:LanguageUnderstandingAppKey"];
        private static readonly string ApiKey = ConfigurationManager.AppSetting["Configurations:LanguageUnderstandingApiKey"];
        private static readonly string LanguageUnderstandingEndpoint = ConfigurationManager.AppSetting["Configurations:LanguageUnderstandingEndpoint"];

        #region Intents
        public const string INTENT_NONE = "None";
        public const string INTENT_GREETING = "Greeting";
        public const string INTENT_CHECK_WEATHER = "CheckWeather";
        public const string INTENT_CHECK_STOCK = "CheckStock";
        #endregion

        #region Entities
        public const string ENTITY_CITY = "builtin.geographyV2.city";
        public const string ENTITY_COUNTRY = "builtin.geographyV2.countryRegion";
        public const string ENTITY_STATE = "builtin.geographyV2.state";
        public const string ENTITY_POI = "builtin.geographyV2.poi";
        public const string EntityStockSymbol = "StockSymbol";
        #endregion

        public static async Task<dynamic> GetAsync(string query)
        {
            // Variable to hold result
            var response = string.Empty;
            var success = true;

            var missingConfigurations = string.IsNullOrWhiteSpace(LanguageUnderstandingEndpoint)
                || string.IsNullOrWhiteSpace(AppKey)
                || string.IsNullOrWhiteSpace(ApiKey);
            if (!missingConfigurations)
            {
                var url = $"{LanguageUnderstandingEndpoint}{AppKey}?timezoneOffset=-360&subscription-key={ApiKey}&q={query}";

                // Create a New HttpClient object and dispose it when done, so the app doesn't leak resources
                using (HttpClient client = new HttpClient())
                {
                    // Call asynchronous network methods in a try/catch block to handle exceptions
                    try
                    {
                        response = await client.GetStringAsync(url);
                    }
                    catch (HttpRequestException e)
                    {
                        response = $"Error: {e.Message}";
                        success = false;
                    }
                }
            }
            else
            {
                response = $"Missing Configurations. Please review setup documentation on the about page.";
                success = false;
            }
            
            return new { response, success };
        }
    }
}
