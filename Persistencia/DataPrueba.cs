using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class DataPrueba
    {
        public static async Task InsertarDatos(CursosOnlineContext context, UserManager<Usuario> usuarioManager)
        {
            //se va insertar siempre y cuando no haya un usuario registrado
            if (!usuarioManager.Users.Any())
            {
                var usuario = new Usuario
                {
                    NombreCompleto = "bryan campos",
                    UserName = "vaxidrez",
                    Email = "vaxi.drez@gmail.com"
                };
                await usuarioManager.CreateAsync(usuario, "Password123$");
            }
        }
    }
}