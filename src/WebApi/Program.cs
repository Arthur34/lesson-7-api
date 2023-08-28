using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebApi
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => 
                {
                    var p = System.Reflection.Assembly.GetEntryAssembly().Location;
                    p = p.Substring(0, p.LastIndexOf(@"\") + 1);

                    webBuilder.UseContentRoot(p);
                    webBuilder.UseStartup<Startup>(); 
                });
    }
}