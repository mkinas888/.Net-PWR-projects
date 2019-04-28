using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Platformy_projekt
{
    class WeatherConnection
    {

        public static async Task<string> DownloadAsJson(string city = "Wroclaw")
        {
            string apikey = "1b6714e500f0cdd864a8b49ec6ac5e45";
            string weatherURL = "https://api.openweathermap.org/data/2.5/weather";
            string apiCall = weatherURL + "?q=" + city + "&apikey=" + apikey;

            Task<string> result;
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(apiCall))
            using (HttpContent content = response.Content)
            {
                result = content.ReadAsStringAsync();
            }
            return await result;

        }
    }
}
