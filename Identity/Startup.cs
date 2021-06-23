using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Identity.DAL.Mongo.Extensions;
using Identity.Stores;
using Identity.Utils;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;

namespace Identity
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }
        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMongoDb(Configuration.GetSection("MongoSettings"));
            
            var builder = services.AddIdentityServer(options =>
            {
                if (this.Environment.IsProduction())
                {
                    options.IssuerUri = About.ProjectUrl;
                    options.MutualTls.Enabled = true;
                }
            });

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
                    await context.Response.WriteAsync(
                        Utils.About.GetHtmlWelcomePage(
                            env.IsDevelopment()
                                ? context.Request.GetEncodedUrl()
                                : About.ProjectUrl
                        )
                    );
                });
            });
        }
    }
}
