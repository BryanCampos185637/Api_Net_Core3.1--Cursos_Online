using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecuta:IRequest<UsuarioData>
        {
            
        }
        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly IJwtGenerador jwtGenerador;
            private readonly IUsuarioSesion usuarioSesion;
            public Manejador(UserManager<Usuario> _userManager, IJwtGenerador _jwtGenerador, IUsuarioSesion _usuarioSesion)
            {
                userManager = _userManager;
                jwtGenerador = _jwtGenerador;
                usuarioSesion = _usuarioSesion;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByNameAsync(usuarioSesion.ObtenerUsuarioSesion());
                var listaRoles = await userManager.GetRolesAsync(usuario);
                return new UsuarioData
                {
                    UserName = usuario.UserName,
                    NombreCompleto = usuario.NombreCompleto,
                    Token = jwtGenerador.CrearToken(usuario,new List<string>(listaRoles)),
                    Imagen = null,
                    Email = usuario.Email
                };
            }
        }
    }
}
