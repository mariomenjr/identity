using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Diagnostics;
using Identity.Utils;

namespace Identity
{
    public class Startup
    {
        private FileVersionInfo About { get; }
        public Startup()
        {
            this.About = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryClients(Config.Clients)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiResources(Config.ApiResources())
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddTestUsers(Config.Users)
                .AddDeveloperSigningCredential();
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
                    string repoUrl = "https://github.com/mariomenjr/identity";
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
