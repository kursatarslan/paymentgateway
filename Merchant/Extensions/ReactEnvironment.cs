using System.IO;
using System.Linq;
using Merchant.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Merchant.Extensions
{
    public static class ReactEnvironment
    {
        public static void AddReactEnvironment<CONFIG>(this IServiceCollection services, string buildPath, CONFIG settings) {

            var indexPath = buildPath + "/index.html";

            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = buildPath;
            });

            //edit index.html file if build path exists. 
            if (File.Exists(indexPath)) {
                
                var file = File.ReadAllText(indexPath);

                typeof(CONFIG).GetProperties().Select(prop => new {
                        ReactEnvNames = prop.GetCustomAttributes(false)
                            .OfType<ReactEnvironmentVarAttribute>()
                            .Select(p => p.VarName).ToList(),
                        Value = prop.GetValue(settings)
                    }).ToList()
                    .ForEach(
                        p => p.ReactEnvNames.ForEach(
                            env => file = file.Replace(@$"%{env}%", p.Value.ToString())));

                File.WriteAllText(indexPath, file);
            }
        }
    }
}