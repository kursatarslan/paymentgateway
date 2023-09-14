using System;
using System.IO;
using System.Net;
using GatewayService;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);


IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("ocelot.json",optional: false, reloadOnChange: true)
    .Build();

builder.Configuration.SetBasePath(Environment.CurrentDirectory);
builder.Host.UseContentRoot(Directory.GetCurrentDirectory());
IdentityModelEventSource.ShowPII = true;
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

var myAllowSpecificOrigins = "myAllowSpecificOrigins";
Action<IdentityServerAuthenticationOptions> paymentGatewayClient = option =>
{
    option.Authority = "http://localhost:5009";
    option.RequireHttpsMetadata = false;
    option.SupportedTokens = SupportedTokens.Both;
    option.ApiName = "paymentgatewayapi";
    option.ApiSecret = "secret";
};

Action<IdentityServerAuthenticationOptions> productClient = option =>
{
    option.Authority = "http://localhost:5009";
    option.RequireHttpsMetadata = false;
    option.SupportedTokens = SupportedTokens.Both;
    option.ApiName = "productapi";
    option.ApiSecret = "secret";
};
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication()
    .AddIdentityServerAuthentication("PaymentGatewayWebApplicationKey", paymentGatewayClient)
    .AddIdentityServerAuthentication("ProductWebApplicationKey", productClient);

builder.Services.AddOcelot(configuration).AddConsul();
builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins, builder => {
        //builder.WithOrigins("http://localhost:800").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
        //builder.SetIsOriginAllowed(origin => true);
    });
});

var app = builder.Build();
var forwardOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    RequireHeaderSymmetry = false
};

forwardOptions.KnownNetworks.Clear();
forwardOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardOptions);
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
IdentityModelEventSource.ShowPII = true;
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(myAllowSpecificOrigins);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseOcelot().Wait();

app.Run();