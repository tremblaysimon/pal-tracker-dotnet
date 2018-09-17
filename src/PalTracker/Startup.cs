using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PalTracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(sp => new WelcomeMessage(
               Configuration.GetValue<string>("WELCOME_MESSAGE", "WELCOME_MESSAGE not configured.")
         ));
         services.AddSingleton(sp => new CloudFoundryInfo(
               Configuration.GetValue<string>("PORT", "PORT not configured."),
               Configuration.GetValue<string>("MEMORY_LIMIT", "MEMORY not configured."),
               Configuration.GetValue<string>("CF_INSTANCE_INDEX", "CF_INSTANCE_INDEX not configured."),
               Configuration.GetValue<string>("CF_INSTANCE_ADDR", "CF_INSTANCE_ADDR not configured.")
         ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
