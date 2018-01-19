using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CourseProjectApp_WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            //Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();


            if(env.IsDevelopment())
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();

        }

        //public IConfiguration Configuration { get; }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddApplicationInsightsTelemetry(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory, IConfiguration configuration)
        {
            loggerfactory.AddConsole(Configuration.GetSection("Logging"));
            loggerfactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            //app.UseMvc(Configurationroute);
            var routename = configuration["Defaultvalue"];
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: routename,
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
        /*
        /// <summary>
        /// Here we can give the routes and configure in usemvc method
        /// </summary>
        /// <param name="obj"></param>
        private void Configurationroute(IRouteBuilder obj)
        {
            obj.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
        }
        */
    }
}
