using chatbot.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace chatbot.Models
{
    public class WeatherModel
    {
        private static readonly string WeatherApiEndpoint = ConfigurationManager.AppSetting["Configurations:DarkSkyEndpoint"];
        private static readonly string WeatherSecretKey = ConfigurationManager.AppSetting["Configurations:DarkSkySecretKey"];
        public static async Task<dynamic> GetAsync(double latitude, double longitude)
        {
            // Variable to hold result
            var response = string.Empty;
            var success = true;

            var missingConfigurations = string.IsNullOrWhiteSpace(WeatherApiEndpoint)
                || string.IsNullOrWhiteSpace(WeatherSecretKey);
            if (!missingConfigurations)
            {
                var url = $"{WeatherApiEndpoint}{WeatherSecretKey}/{latitude},{longitude}";
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
        public WeatherModel(string jsonPayload)
        {
            dynamic forecast = JsonConvert.DeserializeObject(jsonPayload);
            if(forecast != null)
            {
                Latitude = forecast.latitude;
                Longitude = forecast.longitude;
                Timezone = forecast.timezone;
                // Unix timestamp is seconds past epoch
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                Time = dtDateTime.AddSeconds(Convert.ToDouble(forecast.currently.time)).ToLocalTime();
                Summary = forecast.currently.summary;
                Icon = forecast.currently.icon;
                Temperature = Convert.ToDecimal(forecast.currently.temperature);
            }
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal Temperature { get; set; }
        public string Timezone { get; set; }
        public DateTime Time { get; set; }
        public string Summary { get; set; }
        public string Icon { get; set; }
    }
}
