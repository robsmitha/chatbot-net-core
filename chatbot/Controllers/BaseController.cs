using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chatbot.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatbot.Controllers
{
    public class BaseController : Controller
    {
        public string CDNLocation => ConfigurationManager.GetConfiguration("AWSCDN");
        public string BucketName => ConfigurationManager.GetConfiguration("S3BucketName");
        public string ConversationStirng => HttpContext.Session.GetString("Conversation") ?? string.Empty;
        public bool HasSessionConversation => !string.IsNullOrEmpty(ConversationStirng);
    }
}