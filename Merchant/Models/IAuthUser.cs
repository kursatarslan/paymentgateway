namespace Merchant.Models
{
    public interface IAuthUser
    {
        string Status   { get; }
        string AccessToken    { get; }
        string UserName { get; }
        string RefreshToken { get; }
    }
}