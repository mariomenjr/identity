using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Diagnostics;

namespace Identity
{
    public class Startup
    {
        private FileVersionInfo About { get; }
        public Startup()
        {
            this.About = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                // .AddInMemoryIdentityResources(new List<IdentityResource>())
                // .AddInMemoryApiResources(new List<ApiResource>())
                // .AddTestUsers(new List<TestUser>())
                .AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
