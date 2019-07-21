using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Api.Helpers;
using Api.Hubs;
using Api.SignalR.ClientServices;
using Api.SignalR.ClientServices.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ping.Commons.Settings;

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
            // Add CORS Policy - allow requests only from our app's origins
            services.AddCors(options => options.AddPolicy(AddCorsPolicy, builder =>
            {
                builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins(MicroservicesAllowedOrigins)
                .AllowCredentials();
            }));

            // JWT Authentication - use tokens for authenticating and authorizing requests
            var appSettingsSection = Configuration.GetSection("SecuritySettings");

            services.Configure<SecuritySettings>(appSettingsSection); // Dependency Injection of our appsettings security section
            services.Configure<GatewayBaseSettings>(Configuration.GetSection("GatewayBaseSettings"));

            var appSettings = appSettingsSection.Get<SecuritySettings>();
            var secretKey = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(options =>
            {
                // Use Bearer (the below one) authentication scheme
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey), // Crucial: this is what we'll check all tokens against
                    ValidateLifetime = true,
                    LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow, // Check whether a request's token is expired
                };
                // Hook a JWT authentication handler to the OnMessageReceived event (WS connections and SS events)
                //  --- We want to set the token to our Context, so we can use this to authorize requests within hubs etc.
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context => {
                        var accessToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken; // Read and set the token out of the query string
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            // Service layer
            services.AddScoped<IAccountSignalRClient, AccountSignalRClient>();
            services.AddSingleton<IDataSpaceSignalRClient, DataSpaceSignalRClient>();

            // Signalr - add signalr and also use our custom UserClaim identity provider
            services.AddSingleton<IUserIdProvider, UserClaimIdentifierProvider>();
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            // Mvc-Api
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Configure our upload size limit
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

            app.UseAuthentication();

            app.UseStaticFiles();
            
            // Map signalR hubs
            app.UseSignalR(routes =>
            {
                routes.MapHub<AccountHub>("/accounthub", options =>
                {
                    // Set hub's buffer limit in bytes
                    options.ApplicationMaxBufferSize = 100 * 1024;
                });

                routes.MapHub<DataSpaceHub>("/dataspacehub", options =>
                {
                    options.ApplicationMaxBufferSize = 10_000 * 1024;
                });

                routes.MapHub<AuthHub>("/authhub");

                routes.MapHub<ChatHub>("/chathub");
            });

            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //  name: "areas",
                //  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
