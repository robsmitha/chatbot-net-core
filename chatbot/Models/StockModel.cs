using chatbot.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace chatbot.Models
{
    public class StockModel
    {
        private static readonly string AplhaAdvantageApiKey = ConfigurationManager.AppSetting["Configurations:AplhaAdvantageApiKey"];
        private static readonly string AplhaAdvantageApiEndPoint = ConfigurationManager.AppSetting["Configurations:AplhaAdvantageApiEndPoint"];
        private static readonly string TIME_SERIES_INTRADAY = ConfigurationManager.AppSetting["Configurations:AplhaAdvantageTIME_SERIES_INTRADAY"];
        public static async Task<dynamic> GetAsync(string symbol)
        {
            // Variable to hold result
            var response = string.Empty;
            var success = true;
           
            var missingConfigurations = string.IsNullOrWhiteSpace(AplhaAdvantageApiEndPoint)
                || string.IsNullOrWhiteSpace(AplhaAdvantageApiKey)
                || string.IsNullOrWhiteSpace(TIME_SERIES_INTRADAY);
            if (!missingConfigurations)
            {
                var url = $"{AplhaAdvantageApiEndPoint}function={TIME_SERIES_INTRADAY}&symbol={symbol}&interval=5min&apikey={AplhaAdvantageApiKey}";

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
        public StockModel(string jsonPayload)
        {
            dynamic stock = JsonConvert.DeserializeObject(jsonPayload);
            var information = stock["Meta Data"]["1. Information"];
            var symbol = stock["Meta Data"]["2. Symbol"];
            var lastRefreshed = stock["Meta Data"]["3. Last Refreshed"];
            var interval = stock["Meta Data"]["4. Interval"];
            var outputSize = stock["Meta Data"]["5. Output Size"];
            var timeZone = stock["Meta Data"]["6. Time Zone"];
            var timeSeries = stock[$"Time Series ({interval})"][lastRefreshed.ToString()];
            var open = timeSeries["1. open"];
            var high = timeSeries["2. high"];
            var low = timeSeries["3. low"];
            var close = timeSeries["4. close"];
            var volume = timeSeries["5. volume"];

            Information = information;
            Symbol = symbol;
            LastRefreshed = Convert.ToDateTime(lastRefreshed);
            Interval = interval;
            OutputSize = outputSize;
            TimeZone = timeZone;
            Open = Convert.ToDecimal(open);
            High = Convert.ToDecimal(high);
            Low = Convert.ToDecimal(low);
            Close = Convert.ToDecimal(close);
            Volume = Convert.ToInt32(volume);
        }
        public string Information { get; set; }
        public string Symbol { get; set; }
        public DateTime? LastRefreshed { get; set; }
        public string Interval { get; set; }
        public string OutputSize { get; set; }
        public string TimeZone { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public int Volume { get; set; }
    }
}
