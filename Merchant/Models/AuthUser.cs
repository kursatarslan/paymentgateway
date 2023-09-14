namespace Merchant.Models
{
    public class AuthUser : IAuthUser
    {
        public string Status   { get; }
        public string AccessToken    { get; }
        public string RefreshToken    { get; }
        public string UserName { get; }

        public AuthUser(string status, string accesstoken, string userName,string refreshToken)
        {
            Status = status;
            RefreshToken = refreshToken;
            AccessToken = accesstoken;
            UserName = userName;
        }
    }
}