using chatbot.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatbot.Models
{
    public class ThemeViewModel
    {
        public string version { get; set; }
        public IEnumerable<ThemeModel> themes { get; set; }
        public ThemeViewModel() { }
    }
}
