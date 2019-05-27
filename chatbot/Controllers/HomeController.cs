using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using chatbot.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using chatbot.Utilities;

namespace chatbot.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            //for jquery ui autocomplete script include
            ViewBag.CDN = !string.IsNullOrWhiteSpace(CDNLocation) && !string.IsNullOrWhiteSpace(BucketName) 
                ? $"{CDNLocation}{BucketName}" 
                : string.Empty;
            return View();
        }
        public IActionResult List()
        {
            var model = new ChatViewModel();
            if (HasSessionConversation)
            {
                model.Items = JsonConvert.DeserializeObject<List<LUIS>>(ConversationStirng);
            }
            return PartialView(model);
        }
        public IActionResult About()
        {
            ViewData["Message"] = ConfigurationManager.GetString("AboutDescription");

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = ConfigurationManager.GetString("ContactDescription");

            return View();
        }

        public async Task<JsonResult> Speak(string query)
        {
            var result = await LanguageUnderstandingModel.GetAsync(query);
            LUIS luis = new LUIS
            {
                query = query,
                timeStamp = DateTime.Now
            };

            if (result.success)
            {
                try
                {
                    //Deserialize response body into LUIS object
                    luis = JsonConvert.DeserializeObject<LUIS>(result.response);
                    if(luis != null)
                    {
                        await luis.Reply();
                    }
                }
                catch (Exception e)
                {
                    //An error occurred, output error message in conversation flow
                    luis.reply.message = $"Error: {e.Message} "; 
                }
            }
            else
            {
                //The request was not successful, output response in conversation flow
                luis.reply.message = result.response; 
            }

            #region Holding conversation in session
            //variable to hold conversation
            List<LUIS> conversationList;
            //try to get Conversation session key   
            var conversationString = HttpContext.Session.GetString("Conversation"); 
            if (!string.IsNullOrEmpty(conversationString))
            {
                //Deserialize string into conversation list
                conversationList = JsonConvert.DeserializeObject<List<LUIS>>(conversationString);
                //Add to conversation list
                conversationList.Add(luis);  
            }
            else
            {
                //Initialize new list with single item
                conversationList = new List<LUIS>
                {
                    luis
                };
            }
            //Set Conversation session key with serialized list
            HttpContext.Session.SetString("Conversation", JsonConvert.SerializeObject(conversationList));   
            #endregion

            return Json(luis);
        }

        public JsonResult Source(string value)
        {
            var weatherIntents = new List<string>
            {
                "What is the weather in Florida",
                "What is the weather in Paris",
                "What is the weather in New York",
                "What is the weather in Las Vegas",
                "What is the weather in California"
            };

            var stockIntents = new List<string>
            {
                "What is MSFT trading for",
                "What is MSFT",
                "What is the price of MSFT"
            };

            var list = weatherIntents.Where(x => x.ToLower().Contains(value)).ToList();
            list.AddRange(stockIntents.Where(x => x.ToLower().Contains(value)));

            return Json(list);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
