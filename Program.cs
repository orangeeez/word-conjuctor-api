using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Driver;

namespace ConjuctorAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://*:80")
                .Build();

            host.Run();
        }
    }
}
