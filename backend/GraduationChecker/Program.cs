using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace GraduationChecker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args, "http://localhost:43056").Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args, string hostedUrl)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(hostedUrl);
        }
    }
}
