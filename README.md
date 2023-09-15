1. Backend for Frontend (BFF): I began by creating a .NET Backend for Frontend (BFF) server, which served as the intermediary between the React frontend and the various backend services.


2. SSO Integration: I implemented a Single Sign-On (SSO) mechanism to allow the React application to obtain access tokens from the SSO service. These tokens were used for user authentication and authorization.


3. React Frontend: I developed the React frontend, which included a login page that interacted with the SSO service to acquire access tokens. The access tokens were stored securely using state management.


4. Payment Data Request: In the React frontend, I created a payment form to collect payment data. I then sent this payment data as a request to the .NET BFF.


5. API Gateway (Ocelot): Using Ocelot, I built an API Gateway that received the payment data request from the React frontend. Instead of sending the request directly to the bank service, I introduced MassTransit to the flow.


6. MassTransit Integration: Within the API Gateway, I integrated MassTransit to publish the payment request as a message. This message was picked up by a background service, which subscribed to the message and forwarded it to the Payment Gateway.


7. Payment Gateway: The Payment Gateway received the message from MassTransit and processed the payment. Unfortunately, I was not able to complete the next steps, but the idea was to have a Push Notification service listen for the payment confirmation.


8. Push Notification Service: The Push Notification service was intended to listen for payment confirmation events and use SignalR Hub for WebSocket connections to notify the React frontend using Toastr notifications, indicating the successful completion of the payment.

![image](https://github.com/kursatarslan/paymentgateway/assets/9142092/0d069077-e8d0-42e1-847d-5b55ce496e2e)

![image](https://github.com/kursatarslan/paymentgateway/assets/9142092/52f87e1a-05fc-4930-918a-8ca07b374848)

![image](https://github.com/kursatarslan/paymentgateway/assets/9142092/c569774b-78e2-4632-8eec-1d3b16f88681)

![image](https://github.com/kursatarslan/paymentgateway/assets/9142092/1efe8bd8-b60c-4886-9342-4ed4f6993c4d)

![image](https://github.com/kursatarslan/paymentgateway/assets/9142092/6a048c5f-bcfd-4b5e-ace2-a0b96eebeb1c)


# IdentityGatewayDemo
Ocelot + IdentityServer4 to build microservice gateway based on .NET Core plateform

## Migrate commands
dotnet tool install -g dotnet-ef

dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerApplicationDbMigration -c AuthDbContext -o Data/Migrations/IdentityServer/AuthDbContext


dotnet ef database update -c ConfigurationDbContext
dotnet ef database update -c PersistedGrantDbContext
dotnet ef database update -c ApplicationDbContext


dotnet ef migrations script -c ApplicationDbContext 0 20191205154435_InitialIdentityServerApplicationDbMigration -o ../../script/dll/ApplicationDbContext/applicationInitial.sql


## Docker

docker-compose build
docker-compose up
docker-compose down


dotnet ef dbcontext scaffold "Server=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres" Npgsql.EntityFrameworkCore.PostgreSQL -o DbModels -f

//Db first generate entities
dotnet ef dbcontext scaffold "Server=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres" Npgsql.EntityFrameworkCore.PostgreSQL

//generate script
dotnet ef migrations script  --context PersistedGrantDbContext -o persisted.sql

// ssl generate
dotnet dev-certs https -ep "localhost.pfx" -p password --trust


## 1.Config datasource
There are two ways to config this demo code in your local environment:    
### In Memory mode   
  In this model you just need to mock all data in memory and identityserver4 and ocelot will load configuration from it, for ocelot by default if you don't config the database in you ConfigurationService ,it will load gateway config from ocelot.json file:
    
    private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://0.0.0.0:8081")
                .ConfigureAppConfiguration((host,builder) => {
                 builder.SetBasePath(host.HostingEnvironment.ContentRootPath)
                .AddJsonFile("ocelot.json"); })
                .UseStartup<Startup>();
   
  
  and the ocelot samlpe config file will looks like:
      
      {
	  "Routes": [ 
	    {
	      "DownstreamPathTemplate": "", 
	      "UpstreamPathTemplate": "", 
	      "UpstreamHttpMethod": [ "Get","Post" ],
	      "AddHeadersToRequest": {},
	      "FileCacheOptions": { 
	        "TtlSeconds": 10,
	        "Region": ""
	      },
	      "ReRouteIsCaseSensitive": false,
	      "ServiceName": "",
	      "DownstreamScheme": "http",
	      "DownstreamHostAndPorts": [ 
	        {
	          "Host": "localhost",
	          "Port": 8001
	        }
	      ],
	      "RateLimitOptions": { 
	        "ClientWhitelist": [], 
	        "EnableRateLimiting": true,
	        "Period": "1s", 
	        "PeriodTimespan": 15,
	        "Limit": 1 
	      }，
	      "QoSOptions": { 
	        "ExceptionsAllowedBeforeBreaking": 0, 
	        "DurationOfBreak": 0, 
	        "TimeoutValue": 0 
	      }
	    }
	  ],
	  "UseServiceDiscovery": false,
	  "Aggregates": [ 
	    {
	      "ReRouteKeys": [ 
	        "booking",
	        "passenger"
	      ],
	      "UpstreamPathTemplate": "/api/getbookingpassengerinfo" 
	    },
	  "GlobalConfiguration": { 
	    "BaseUrl": "https://localhost:5000" 
	  }
	} 
     
 for more detail configuration you can find it in official document:
 [Ocelot](https://ocelot.readthedocs.io/en/latest/introduction/gettingstarted.html) .    
  for identityservice part you only need to change some code in you startup.cs file:
  
     public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // 1.config data in memory
            //ConfigService.ConfigServiceInMemory(services);

            //2.config data in database
            ConfigServiceInSqlServer(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        } 
 

for ocelot we defined db schema in our code, you can generate database shcema by EntityFramewrok Code, in your GatewayService project directory execute these command to do migrate:
    
    dotnet ef migrations add [Migration name]    
    dotnet ef database update
after schema migrate successfully we need to init config data to db by these script,please notice that the Host should be your local machine ip address
    
   
	 insert into GlobalConfigurations(GatewayName,RequestIdKey,IsDefault,InfoStatus)
	 values('测试网关','test_gateway',1,1);
	

	 insert into ReRoutes values(1,'/api/getproduct','[ "GET" ]','','http','/product','[{"Host": "192.168.191.3","Port": 5002 }]',
	 '{"AuthenticationProviderKey": "ProductIdentityKey","AllowScopes": [ ]}','','','','','','',0,1,'');
	
	 insert into ReRoutes values(1,'/api/getorder','[ "GET" ]','','http','/api/order','[{"Host": "192.168.191.3","Port": 5001 }]',
	 '{"AuthenticationProviderKey": "OrderIdentityKey","AllowScopes": [ ]}','','','','','','',0,1,'');

for identityserver4 official already publish nuget package named IdentitySeever4.EntityFramework.Storage, you can install this package to you project and then do db migrate to generate schema by these command:
    
    dotnet ef migrations add [Migration name] -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb

	dotnet ef database update --context PersistedGrantDbContext
	
	
	dotnet ef migrations add [Migration name] -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
	
	dotnet ef database update --context ConfigurationDbContext

we added test data in code, when first tome application start up these data will init to database, we don't need to init it manually:
    
    public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("productapi", "this is product api"),
                new ApiResource("orderapi", "this is order api")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "product",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("productsecret".Sha256())
                    },

                    AllowedScopes = new [] { "productapi",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile }
                },
                new Client
                {
                    ClientId = "order",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("ordersecret".Sha256())
                    },

                    AllowedScopes = new [] { "orderapi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile }
                }
            };
        }
  before you start application you need to change db connectionstring in appsetting.json:
      
      "DefaultConnection": "Server=192.168.191.3,1450;Database=GatewayServer;User Id=sa;Password=Password123;MultipleActiveResultSets=True;"
## 2.Start script
we write down some start script in go file in solution directory,when you first time to run this project you need to build docker image, so you can use ./go start command to start whole application, after that you can use command ./go up to start the instance which already built.
     
## 3.Test result
if we use postman or other client to access the api by gateway, we will see that:
![Unauthorize](https://github.com/dotNetXA/IdentityGatewayDemo/blob/master/unauthorized.jpg "Optional title")

which means for now our api is protected by identityserver in gateway    
we can only access it by valid token ,so let's request token first:
![Unauthorize](https://github.com/dotNetXA/IdentityGatewayDemo/blob/master/generatetoken.jpg "Optional title")
and we access the api again with the token, we can get what we expected
![Unauthorize](https://github.com/dotNetXA/IdentityGatewayDemo/blob/master/authorized.jpg "Optional title")

