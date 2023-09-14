using System.Threading.Tasks;

namespace Merchant.Hubs
{
    public interface IUsersHub
    {
        Task UserLogin();
        Task UserLogout();
        Task CloseAllConnections(string reason);
    }
}