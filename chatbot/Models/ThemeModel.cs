using chatbot.Utilities;
using System.Net.Http;
using System.Threading.Tasks;

namespace chatbot.Models
{
    public class ThemeModel
    {
        public static string ThemesEndPoint = ConfigurationManager.AppSetting["Configurations:ThemesEndpoint"];
        public string name { get; set; }
        public string description { get; set; }
        public string preview { get; set; }
        public string thumbnail { get; set; }
        public string css { get; set; }
        public string cssMin { get; set; }
        public string cssCdn { get; set; }
        public string scss { get; set; }
        public string scssVariables { get; set; }
        public static async Task<dynamic> GetAsync()
        {
            // Variable to hold result
            var response = string.Empty;
            var success = true;

            var missingConfigurations = string.IsNullOrWhiteSpace(ThemesEndPoint);
            if (!missingConfigurations)
            {
                // Create a New HttpClient object and dispose it when done, so the app doesn't leak resources
                using (HttpClient client = new HttpClient())
                {
                    // Call asynchronous network methods in a try/catch block to handle exceptions
                    try
                    {
                        response = await client.GetStringAsync(ThemesEndPoint);
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
