using DistributedProcess.Clients;
using DistributedProcess.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DistributedProcess
{
    public class Startup
    {
        private const string LeaderManagerUrlSettingKey = "LeaderManagerUrl";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IMasterManager, MasterManager>();
            services.AddSingleton<ILeaderManagerClient, LeaderManagerClient>();

            services.AddHttpClient<ILeaderManagerClient, LeaderManagerClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration[LeaderManagerUrlSettingKey]);
                client.Timeout = TimeSpan.FromMinutes(1);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
