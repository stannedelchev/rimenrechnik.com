using System;
using System.Globalization;
using System.Net;
using Dapper.FastCrud;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RhymeDictionary.Model.Services;

namespace RhymeDictionary
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            OrmConfiguration.DefaultDialect = SqlDialect.PostgreSql;

            // Add framework services.
            WordsService CreateWordsService(IServiceProvider _) => new WordsService(this.Configuration.GetConnectionString("Default"));

            services.AddMvc();
            services.AddScoped<IWordsService>(CreateWordsService);
            services.AddSingleton<IRhymesService>(sp => new InMemoryRhymesService(CreateWordsService(sp)));
            services.AddScoped<IWordSuggestionsService>(_ => new WordSuggestionsService(this.Configuration.GetConnectionString("Default")));
            services.AddScoped<ISiteCommentsService>(_ => new SiteCommentsService(this.Configuration.GetConnectionString("Default")));
            services.AddScoped<IStatisticsService>(_ => new StatisticsService(this.Configuration.GetConnectionString("Default")));

            var authConfigSection = this.Configuration.GetSection("Auth");
            var idsConfig = new IdentityServerConfig(authConfigSection);
            services
                .AddIdentityServer(c => c.PublicOrigin = authConfigSection["IdentityServerOptions:Authority"])
                .AddDeveloperSigningCredential(persistKey: false)
                .AddInMemoryApiResources(idsConfig.GetApiResources())
                .AddInMemoryClients(idsConfig.GetClients());

            services
                .AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(idsConfig.GetAuthOptions);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var bgCulture = new CultureInfo("bg");
            var supportedCultures = new[] { bgCulture };
            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(bgCulture, bgCulture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseForwardedHeaders(new ForwardedHeadersOptions()
                {
                    ForwardedHeaders = ForwardedHeaders.All,
                    RequireHeaderSymmetry = false,
                    ForwardLimit = null
                });
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseIdentityServer();
        }
    }
}
