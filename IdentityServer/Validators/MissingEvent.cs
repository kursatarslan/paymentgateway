using Duende.IdentityServer.Events;

namespace IdentityServer.Validators;

public class MissingEvent : Event
{
    public MissingEvent(string userName, string error, bool interactive = true, string clientId = null)
        : base(EventCategories.Error,
            "missing failure",
            EventTypes.Failure, 
            EventIds.UnhandledException,
            error)
    {
        UserName = userName;
        ClientId = clientId;
        Endpoint = "resourceowner";
            
    }
        
    public string UserName { get; set; }
        
    public string Endpoint { get; set; }
        
    public string ClientId { get; set; }
}