using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Diagnostics;
using Identity.DAL.Mongo.Settings;
using Identity.DAL.Repository.Managers;
using Identity.DAL.Repository.Services;
using Identity.Stores;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Identity
{
    public class Startup
    {
        private FileVersionInfo About { get; }
        private IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            this.About = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MongoSettings>(Configuration.GetSection("MongoSettings"));
            
            services.AddSingleton<IMongoSettings>(srvProvider =>
                srvProvider.GetRequiredService<IOptions<MongoSettings>>().Value);
            
            services.AddTransient<IClientService, ClientManager>();
            
            var builder = services.AddIdentityServer();

            builder
                // .AddInMemoryClients(Config.Clients)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiResources(Config.ApiResources())
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddTestUsers(Config.Users)
                .AddDeveloperSigningCredential();
            
            builder.Services.AddTransient<IClientStore, ClientStore>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    string repoUrl = Environment.GetEnvironmentVariable("REPO_URL");
                    await context.Response.WriteAsync(
                        "<html><body>" + 
                        string.Join(
                            "<br />",
                            new string[]
                            {
                                $"{this.About.ProductName}/{this.About.FileVersion} @ {DateTimeOffset.Now.ToString()}",
                                $"Repo: <a href=\"{repoUrl}\">{repoUrl}</a>",
                            }
                        ) + 
                        "</body></html>"
                    );
                });
            });
        }
    }
}
