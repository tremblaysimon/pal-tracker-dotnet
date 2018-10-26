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

using Microsoft.EntityFrameworkCore;

using Steeltoe.Management.CloudFoundry;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Common.HealthChecks;
using Steeltoe.CloudFoundry.Connector.PostgreSql.EFCore;

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
            

            Console.WriteLine("Demarrage de l'application...");

            services.AddScoped<ITimeEntryRepository, MySqlTimeEntryRepository>();
           

            services.AddDbContext<TimeEntryContext>(options => options.UseNpgsql(Configuration));
 Console.WriteLine("Steeltoe connection string: " + Configuration.GetConnectionString("abtest"));


            //services.AddEntityFrameworkNpgsql().AddDbContext<TimeEntryContext>(options => options.UseNpgsql(Configuration.GetConnectionString("abtest")));

            //services.AddEntityFrameworkNpgsql().AddDbContext<TimeEntryContext>(options => options.UseNpgsql("User ID=dma7217ad2a0379d29094603fb58f0442551f14;Password=GjFfDQ!N9E$tX3__58$F;Server=10.220.229.228;Port=5432;Database=abtest;Pooling=true;"));

            services.AddCloudFoundryActuators(Configuration);

            services.AddSingleton<IHealthContributor, TimeEntryHealthContributor>();

            services.AddSingleton<IOperationCounter<TimeEntry>, OperationCounter<TimeEntry>>();

            services.AddSingleton<IInfoContributor, TimeEntryInfoContributor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseCloudFoundryActuators();
        }
    }
}
