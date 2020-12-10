using System.Net.Http;

namespace Almostengr.FalconPiMonitor
{
    public class ApiHelper
    {
        public static HttpClient HttpClient { get; set; }

        public ApiHelper()
        {
            HttpClient = new HttpClient();
            // HttpClient.DefaultRequestHeaders.Accept.Add(new Medi)
        }
    }
}