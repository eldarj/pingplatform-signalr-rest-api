﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = 100_000 * 1024; // Set Kestrel's upload limit in bytes
                })
                .UseUrls(new string[] { "https://192.168.1.3:44380", "http://192.168.1.3:18458" }); // Set the http and https urls


    }
}
