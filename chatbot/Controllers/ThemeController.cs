using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using chatbot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace chatbot.Controllers
{
    public class ThemeController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var result = await ThemeModel.GetAsync();
            var model = new ThemeViewModel();
            if (result.success)
            {
                ViewData["Message"] = "Draw me like one of your french girls.";
                model = JsonConvert.DeserializeObject<ThemeViewModel>(result.response);
            }
            else
            {
                ViewData["Message"] = result.response;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apply(string themeCdn)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("themeCdn", themeCdn);
            }

            return RedirectToAction("Index");
        }
    }
}