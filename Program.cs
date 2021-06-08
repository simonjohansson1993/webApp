using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using environment_crime.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace myFirstWebApplication {
  public class Program {
    public static void Main(string[] args) {
      var host = CreateHostBuilder(args).Build();

      InitializeDatabase(host);
      host.Run();
    }

    private static void InitializeDatabase(IHost host) {
      using (var scope = host.Services.CreateScope()) {
        var services = scope.ServiceProvider;
        try {
          
          DBInitializer.EnsurePopulated(services);
          IdentityInitializer.EnsurePopulated(services).Wait();

        }
        catch (Exception e) {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(e, "Ett fel uppstod vid fyllning av data");
        }
      }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => {
              webBuilder.UseStartup<Startup>();
            });
  }
}
