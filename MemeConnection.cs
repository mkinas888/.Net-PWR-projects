using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Platformy_projekt
{
    class MemeConnection
    {
        public static async Task<string> DownloadJsonAsync(string subreddit = "memes")
        {
            string apiCall = "https://www.reddit.com/r/" + subreddit +  "/top.json";
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
