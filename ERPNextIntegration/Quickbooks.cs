using System;
using System.IO;
using System.Threading.Tasks;
using ERPNextIntegration.QBO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QuickBooksSharp;

namespace ERPNextIntegration
{
    public static class Quickbooks
    {
        private static AuthenticationService _authenticationService;
        public static DataService DataService;
        private static string _clientId;
        private static string _clientSecret;
        private static Token _tokensInMemory;
        private static Token TokensOnDisk
        {
            get
            {
                using var r = new StreamReader(Directory.GetCurrentDirectory() + @"\tokens.txt");
                var json = r.ReadToEnd();
                var tokenFile = JsonConvert.DeserializeObject<Token>(json);
                return tokenFile;
            }
            set
            {
                using var file = File.CreateText(Directory.GetCurrentDirectory() + @"\tokens.txt");
                var serializer = new JsonSerializer();
                serializer.Serialize(file, value);
            }
        }
        public static void InitializeQuickbooksClient(IConfiguration configuration)
        {
            _clientId = configuration.GetValue<string>("Authentication:ClientId");
            _clientSecret = configuration.GetValue<string>("Authentication:ClientSecret");
            long.TryParse(configuration.GetValue<string>("Authentication:realmId"), out var realmId);
            _authenticationService = new AuthenticationService();
            _tokensInMemory = TokensOnDisk;
            DataService = new DataService(_tokensInMemory.AccessToken, realmId, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development");
        }

        public static async Task RefreshTokens()
        {
            if (_tokensInMemory.LastAccessTokenRefresh < DateTime.Now.Subtract(TimeSpan.FromMinutes(59)))
            {
                var tokenResponse = await _authenticationService.RefreshOAuthTokenAsync(_clientId, _clientSecret, _tokensInMemory.RefreshToken);
                _tokensInMemory.LastAccessTokenRefresh = DateTime.Now;
                _tokensInMemory.LastRefreshTokenRefresh = _tokensInMemory.RefreshToken == tokenResponse.refresh_token
                    ? DateTime.Now
                    : _tokensInMemory.LastRefreshTokenRefresh;
                _tokensInMemory.AccessToken = tokenResponse.access_token;
                _tokensInMemory.RefreshToken = tokenResponse.refresh_token;
                TokensOnDisk = _tokensInMemory;
            }
        }
    }
}