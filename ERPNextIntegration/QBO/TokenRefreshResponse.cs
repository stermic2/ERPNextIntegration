using System.Security.Cryptography;

namespace ERPNextIntegration.QBO
{
    public class TokenRefreshResponse
    {
        public string refreshToken { get; set; }
        public string accessToken { get; set; }
        public int expires_in { get; set; }
        public int x_refresh_token_expires_in { get; set; }
    }
}