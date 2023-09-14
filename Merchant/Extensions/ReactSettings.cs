using Merchant.Attributes;

namespace Merchant.Extensions
{
    public class ReactSettings {

        [ReactEnvironmentVar("REACT_APP_API_URL")]
        public string ApiUrl { get; set; }

        [ReactEnvironmentVar("REACT_APP_APIGATEWAY_URL")]
        public string ApiGatewayUrl { get; set; }
    }
}