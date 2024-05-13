using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNotesApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!!!");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

/*
 * System.AggregateException: 'Some services are not able to be constructed (Error while validating the service descriptor 'ServiceType: ExcepionHandling.GlobalExceptionHandlingMiddleware Lifetime: Transient ImplementationType: ExcepionHandling.GlobalExceptionHandlingMiddleware': Unable to resolve service for type 'Microsoft.AspNetCore.Http.RequestDelegate' while attempting to activate 'ExcepionHandling.GlobalExceptionHandlingMiddleware'.)'

 * */
