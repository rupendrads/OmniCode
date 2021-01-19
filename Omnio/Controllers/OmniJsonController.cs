using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Omnio.model;

namespace Omnio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OmniJsonController: ControllerBase
    {
        [HttpGet]
        public IEnumerable<playerDetails> Get()
        {
            var jsonString = System.IO.File.ReadAllText("./Data/Data.json");
            var jsonModel = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<playerDetails>>(jsonString);
            var modelJson = System.Text.Json.JsonSerializer.Serialize(jsonModel);
            return jsonModel;
        }

        [HttpGet("Score/{number}")]      
        public IEnumerable<playerDetails> Score(int number)
        {
            var jsonString = System.IO.File.ReadAllText("./Data/Data.json");
            var jsonModel = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<playerDetails>>(jsonString);
            var jsonFilter = jsonModel.Where(a => a.Score > number);
            var modelJson = System.Text.Json.JsonSerializer.Serialize(jsonModel);
            
            return jsonFilter;
        }

        [HttpPost("UpdateScore/{player}/{number}")]
        public IEnumerable<playerDetails> UpdateScore(string player,int number)
        {
            string filepath = "./Data/Data.json";
            string result = string.Empty;
            using (StreamReader r = new StreamReader(filepath))
            {
                var json = r.ReadToEnd();
                var jsonModel = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<playerDetails>>(json);
         
                foreach (var item in jsonModel.Where(a=>a.Player==player))
                {
                    item.Score = number;
                }
                result = System.Text.Json.JsonSerializer.Serialize(jsonModel);
            }

            System.IO.File.WriteAllText(filepath, result);
                               
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<playerDetails>>(result);
        }
    }
}
