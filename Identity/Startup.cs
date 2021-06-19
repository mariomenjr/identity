using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Diagnostics;
using Identity.DAL.Mongo.Extensions;
using Identity.Stores;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;

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
            services.AddMongoDb(Configuration.GetSection("MongoSettings"));
            
            var builder = services.AddIdentityServer();

            builder.AddDeveloperSigningCredential();
            
            builder.Services.AddTransient<IClientStore, ClientStore>();
            builder.Services.AddTransient<IResourceStore, ResourceStore>();
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
