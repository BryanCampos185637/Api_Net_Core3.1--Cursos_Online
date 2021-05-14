using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using Aplicacion.Contratos;
using System.Collections.Generic;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(p => p.Email).NotEmpty();
                RuleFor(p => p.Password).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly SignInManager<Usuario> signInManager;
            private readonly IJwtGenerador jwtGenerador;
            public Manejador(UserManager<Usuario> _userManager, SignInManager<Usuario> _signManager, IJwtGenerador _jwtGenerador)
            {
                userManager = _userManager;
                signInManager = _signManager;
                jwtGenerador = _jwtGenerador;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //validamos que el email existe
                var usuario = await userManager.FindByEmailAsync(request.Email);
                if (usuario == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.Unauthorized);
                }
                //validamos la contraseña
                var rpt = await signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                var listaRoles = await userManager.GetRolesAsync(usuario);
                if (rpt.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Email = usuario.Email,
                        Token = jwtGenerador.CrearToken(usuario, new List<string>(listaRoles)),
                        UserName = usuario.UserName,
                        Imagen = null
                    };
                }
                throw new ManejadorExcepcion(System.Net.HttpStatusCode.Unauthorized);
            }
        }
    }
}