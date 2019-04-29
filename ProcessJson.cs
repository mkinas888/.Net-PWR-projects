using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Platformy_projekt
{
    class ProcessJson
    {
        static private int MemeIndeks = 0;
        public static Dictionary<string, string> NextMeme(JObject JMeme)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            var children = JMeme["data"]["children"];

            data["score"] = children[MemeIndeks]["data"]["score"].ToString();
            data["author"] = children[MemeIndeks]["data"]["author"].ToString();
            data["url"] = children[MemeIndeks]["data"]["url"].ToString();

            if (MemeIndeks++ > 23) MemeIndeks = 0;
            

            return data;
        }

        public static Dictionary<string, string> Weather(JObject JWeather)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
			string conditionState = "";
            if (JWeather["cod"].ToString() == "200")
            {
				foreach (var condition in JWeather["weather"])
				{
					conditionState += condition["main"].ToString() + " ";
				}
				data["cod"] = JWeather["cod"].ToString();
                data["city"] = JWeather["name"].ToString();
                data["temp"] = JWeather["main"]["temp"].ToString();
				data["pressure"] = JWeather["main"]["pressure"].ToString();
				data["speed"] = JWeather["wind"]["speed"].ToString();
				data["condition"] = conditionState;

                return data;
            }
            else
            {
                data["cod"] = JWeather["cod"].ToString();
                return data;
            }
            
        }


    }
}
