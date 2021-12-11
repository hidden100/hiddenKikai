using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalR.Hosting.Self;

namespace shinhayasa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string url = "http://localhost:44316/gamehub/";
            var server = new Server(url);

            // Map the default hub url (/signalr)
            server.MapHubs();

            // Start the server
            server.Start();

            Console.WriteLine("Server running on {0}", url);

            // Keep going until somebody hits 'x'
            while (true)
            {
                ConsoleKeyInfo ki = Console.ReadKey(true);
                if (ki.Key == ConsoleKey.X)
                {
                    break;
                }
            }
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
