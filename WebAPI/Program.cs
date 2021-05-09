using System;
using Dominio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistencia;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //ejecuta nuestro context de persistencia
            var hostServer = CreateHostBuilder(args).Build();
            using (var ambiente = hostServer.Services.CreateScope())
            {
                var services = ambiente.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<CursosOnlineContext>();
                    context.Database.Migrate();//justo aqui se ejecuta la migración
                    //creamos el usuario
                    var userManager = services.GetRequiredService<UserManager<Usuario>>();
                    DataPrueba.InsertarDatos(context, userManager).Wait();
                }
                catch (Exception e)
                {
                    var loggin = services.GetRequiredService<ILogger<Program>>();
                    loggin.LogError(e, "Ocurrio un error en la migración");
                }
            }
            hostServer.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}