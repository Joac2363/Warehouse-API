using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Warehouse_WPF_App.DataAccess
{
    public class DataManager
    {
        static string _baseUrl = "https://localhost:7210";

        private static HttpClient sharedClient = new HttpClient();
        public DataManager()
        {
        }

        public static async Task<T?> GET<T>(string endpoint)
        {
            HttpResponseMessage response = await sharedClient.GetAsync(_baseUrl+endpoint);
            //if (response.IsSuccessStatusCode)
            //{
                string content = await response.Content.ReadAsStringAsync();
                T? deserialized = JsonSerializer.Deserialize<T>(content);
                return deserialized;
            //}

        }
    }
}
