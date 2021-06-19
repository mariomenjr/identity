using Identity.DAL.Mongo.Settings;
using Identity.DAL.Repository.Managers;
using Identity.DAL.Repository.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Identity.DAL.Mongo.Extensions
{
    public static class MongoExtension
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfigurationSection mongoSettings)
        {
            services.Configure<MongoSettings>(mongoSettings);
            
            services.AddSingleton<IMongoSettings>(srvProvider =>
                srvProvider.GetRequiredService<IOptions<MongoSettings>>().Value);
            
            services.AddTransient<IClientService, ClientManager>();
            services.AddTransient<IIdentityResourceService, IdentityResourceManager>();
            services.AddTransient<IApiScopeService, ApiScopeManager>();
            services.AddTransient<IApiResourceService, ApiResourceManager>();

            return services;
        }
    }
}