using System;
using System.IO;
using System.Threading.Tasks;
using ERPNextIntegration.Dtos.QBO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
        private static long _realmId;
        private static Token TokensOnDisk
        {
            get
            {
                if (!File.Exists(Directory.GetCurrentDirectory() + @"\tokens.txt"))
                    return new Token();
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
        public static void InitializeQuickbooksClient(IConfiguration configuration, IWebHostEnvironment env)
        {
            _clientId = configuration.GetValue<string>("Authentication:ClientId");
            _clientSecret = configuration.GetValue<string>("Authentication:ClientSecret");
            long.TryParse(configuration.GetValue<string>("Authentication:realmId"), out _realmId);
            _authenticationService = new AuthenticationService();
            _tokensInMemory = TokensOnDisk;
            DataService = new DataService(_tokensInMemory.AccessToken, _realmId, env.IsDevelopment());
        }

        public static async Task RefreshTokens()
        {
            if (_tokensInMemory.LastAccessTokenRefresh < DateTime.Now.Subtract(TimeSpan.FromMinutes(59)))
                await ForceRefresh();
        }

        public static async Task ForceRefresh()
        {
            var tokenResponse =
                await _authenticationService.RefreshOAuthTokenAsync(_clientId, _clientSecret, _tokensInMemory.RefreshToken);
            _tokensInMemory.LastAccessTokenRefresh = DateTime.Now;
            _tokensInMemory.LastRefreshTokenRefresh = _tokensInMemory.RefreshToken == tokenResponse.refresh_token
                ? DateTime.Now
                : _tokensInMemory.LastRefreshTokenRefresh;
            _tokensInMemory.AccessToken = tokenResponse.access_token;
            _tokensInMemory.RefreshToken = tokenResponse.refresh_token;
            TokensOnDisk = _tokensInMemory;
            DataService = new DataService(_tokensInMemory.AccessToken, _realmId, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development");
        }
    }
}