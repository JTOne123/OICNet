﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OICNet.Server.Builder;
using OICNet.Server.Hosting;
using OICNet.Server.ResourceRepository;

namespace OICNet.Server.Example
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                // Optional: Provide our OIC configuration here, or use the detault. if omitted, the default will be used.
                .AddSingleton(new OicConfiguration(new MyResourceResolver()))
                .AddSingleton<IOicResourceRepository, MyResources>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseResourceRepository("test");
            app.UseResourceRepository(options => 
            {
                options.RequestPath = "youre";
                options.UseResourceRepository<MyOtherResources>("/awesome");
            });
        }
    }
}