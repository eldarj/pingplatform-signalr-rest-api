using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Hubs;
using Api.SignalR.ClientServices;
using Api.SignalR.ClientServices.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private readonly string AddCorsPolicy = "AllowedOrigins";
        private readonly string[] MicroservicesAllowedOrigins = new string[] {
            "https://localhost:3000",
            "https://localhost:44353"
        };

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy(AddCorsPolicy, builder =>
            {
                builder
                .AllowAnyHeader()
                .WithOrigins(MicroservicesAllowedOrigins)
                .AllowCredentials();
            }));

            services.AddSingleton<IAccountSignalRClient, AccountSignalRClient>();
            services.AddSingleton<IDataSpaceSignalRClient, DataSpaceSignalRClient>();
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 104_857_600;
                x.ValueLengthLimit = int.MaxValue;
                //x.MultipartBodyLengthLimit = int.MaxValue;
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(AddCorsPolicy);
            app.UseStaticFiles();
            
            app.UseSignalR(routes =>
            {
                routes.MapHub<AccountHub>("/accounthub", options =>
                {
                    // set to 10mb max
                    options.ApplicationMaxBufferSize = 15000 * 1024;
                });
                routes.MapHub<AuthHub>("/authhub");
                routes.MapHub<DataSpaceHub>("/dataspacehub");
                //routes.MapHub<ContactsHub>("/contactshub");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
