using Microsoft.Extensions.Configuration;
using RestSharp;

namespace ERPNextIntegration
{
    public static class ErpNext
    {
        public static RestClient Client;
        private static string _apiKey;
        private static string _apiSecret;
        public static void InitializeErpClient(IConfiguration configuration)
        {
            Client = new RestClient(configuration.GetValue<string>("Urls:ErpNext"));
            _apiKey = configuration.GetValue<string>("ERPNext:api_key");
            _apiSecret = configuration.GetValue<string>("ERPNext:api_secret");
            Client.AddDefaultHeader("Authorization", "token " + _apiKey + ":" + _apiSecret);
        }
    }
}