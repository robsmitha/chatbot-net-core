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


    public class LUIS
    {
        public string query { get; set; } = string.Empty;
        public TopScoringIntent topScoringIntent { get; set; } = new TopScoringIntent();
        public List<Entity> entities { get; set; } = new List<Entity>();
        public Reply reply { get; set; } = new Reply();
        public DateTime timeStamp { get; set; } = DateTime.Now;

        public async Task<bool> Reply()
        {
            var success = false;
            //Determine what the query was intenting to do
            switch (topScoringIntent.intent)
            {
                case LanguageUnderstandingModel.INTENT_GREETING:
                    //Check degree of certainty that the intent is a correct assumption
                    if (topScoringIntent.score > .9700001)
                    {
                        success = Greeting();
                    }
                    else
                    {
                        reply.message = "I didn't quite catch that.";
                    }
                    break;
                case LanguageUnderstandingModel.INTENT_CHECK_STOCK:
                    if (topScoringIntent.score > .5)
                    {
                        success = await Stock();
                    }
                    else
                    {
                        reply.message = "I didn't quite catch that.";
                    }
                    break;
                case LanguageUnderstandingModel.INTENT_CHECK_WEATHER:
                    if (topScoringIntent.score > .5)
                    {
                        success = await Weather();
                    }
                    else
                    {
                        reply.message = "I didn't quite catch that.";
                    }
                    break;
                case LanguageUnderstandingModel.INTENT_NONE:
                default:
                    reply.message = "Pump the brakes, I didn't understand any of that.";
                    break;
            }
            return success;
        }

        private bool Greeting()
        {
            var success = true;
            reply.message = "Hey!";
            return success;
        }
        private async Task<bool> Stock()
        {
            var success = true;
            var symbol = string.Empty;
            foreach (var e in entities)
            {
                switch (e.type)
                {
                    case LanguageUnderstandingModel.EntityStockSymbol:
                        symbol = e.entity;
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(symbol))
            {
                var stockPayload = await StockModel.GetAsync(symbol);
                if (stockPayload.success)
                {
                    var stock = new StockModel(stockPayload.response);
                    reply.message = $"I checked the stock {stock.Symbol}. Open: {stock.Open}. High: {stock.High}. Low: {stock.Low}. Close: {stock.Close}. Volume: {stock.Volume}";
                }
                else
                {
                    reply.message = stockPayload.response;
                    success = false;
                }
            }
            else
            {
                reply.message = "I did not understand that stock symbol.";
                success = false;
            }
            return success;
        }

        private async Task<bool> Weather()
        {
            var success = true;
            var city = string.Empty;
            var street = string.Empty;
            var state = string.Empty;
            var country = string.Empty;
            foreach (var e in entities)
            {
                switch (e.type)
                {
                    case LanguageUnderstandingModel.ENTITY_CITY:
                        city = e.entity;
                        break;
                    case LanguageUnderstandingModel.ENTITY_COUNTRY:
                        country = e.entity;
                        break;
                    case LanguageUnderstandingModel.ENTITY_STATE:
                        state = e.entity;
                        break;
                    case LanguageUnderstandingModel.ENTITY_POI:
                        street = e.entity;
                        break;
                }
            }
            var address = $"{street} {city} {state} {country}".Trim();
            if (!string.IsNullOrWhiteSpace(address))
            {
                var locationPayload = await LocationModel.GetAsync(address);
                if (locationPayload.success)
                {
                    var location = new LocationModel(locationPayload.response);
                    var weatherPayload = await WeatherModel.GetAsync(latitude: location.Latitude, longitude: location.Longitude);
                    if (weatherPayload.success)
                    {
                        var weather = new WeatherModel(weatherPayload.response);
                        reply.message = $"I checked the weather in {location.FormattedAddress}. Here is the summary: {weather.Summary}. Current temperature is {weather.Temperature}";
                    }
                    else
                    {
                        reply.message = locationPayload.response;
                        success = false;
                    }
                }
                else
                {
                    reply.message = locationPayload.response;
                    success = false;
                }
            }
            else
            {
                reply.message = "I did not understand that location.";
                success = false;
            }
            return success;
        }
    }
    public class TopScoringIntent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }

    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
    }
    public class Reply
    {
        public string message { get; set; }
    }
}
