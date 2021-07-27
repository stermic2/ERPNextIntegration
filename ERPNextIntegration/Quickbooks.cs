using System;
using ERPNextIntegration.QBO;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using Simple.OData.Client;

namespace ERPNextIntegration
{
    public static class Quickbooks
    {
        public static QuickbooksClient Client;
        public static RestClient QboLoginClient;
        public static void InitializeQuickbooksClient(IConfiguration configuration)
        {
            QboLoginClient = new RestClient(configuration.GetValue<string>("Urls:QboRefresh"));
            Client = new QuickbooksClient(new ODataClientSettings
            {
                BaseUri = new Uri(configuration.GetValue<string>("Urls:Qbo")),
                PayloadFormat = ODataPayloadFormat.Json,
                RequestTimeout = new TimeSpan(0, 1, 0),
                BeforeRequest = request =>
                {
                    if (Client.LastQboLogin < DateTime.Now.Subtract(TimeSpan.FromMinutes(59)))
                    {
                        request.Headers.Add("Authorization", "Bearer " + Client.AccessToken);
                        request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        request.Headers.Add("Accept", "application/json");
                        request.Headers.Add("Cache-Control", "no-cache");
                        request.Properties.Add("grant_type","refresh_token");
                        request.Properties.Add("refresh_token",Client.RefreshToken);
                        var req = new RestRequest(Client.RefreshToken)
                            .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                            .AddHeader("Accept", "application/json")
                            .AddHeader("Cache-Control", "no-cache")
                            .AddParameter("grant_type", "refresh_token")
                            .AddParameter("refresh_token", Client.RefreshToken);
                        var result = QboLoginClient.Post<TokenRefreshResponse>(req);
                        if (result.IsSuccessful)
                        {
                            Client.AccessToken = result.Data.accessToken;
                            Client.RefreshToken = result.Data.refreshToken;
                        }
                    }
                    request.Headers.Add("Accept","application/json");
                    request.Headers.Add("Authorization", "Bearer " + Client.AccessToken);
                }
            });
        }
    }
}