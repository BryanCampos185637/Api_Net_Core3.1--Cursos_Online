using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuarioActualizar
    {
        public class Ejecuta:IRequest<UsuarioData> 
        {
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
        }
        public class EjecutaValidator : AbstractValidator<Ejecuta>
        {
            public EjecutaValidator()
            {
                RuleFor(p => p.Apellidos).NotEmpty();
                RuleFor(p => p.Nombre).NotEmpty();
                RuleFor(p => p.Email).NotEmpty();
                RuleFor(p => p.Password).NotEmpty();
                RuleFor(p => p.UserName).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly CursosOnlineContext context;
            private readonly IJwtGenerador jwtGenerador;
            private readonly IPasswordHasher<Usuario> passwordHasher;
            public Manejador(UserManager<Usuario> user, CursosOnlineContext cursosOnlineContext, IJwtGenerador jwt, IPasswordHasher<Usuario> password)
            {
                userManager = user;
                context = cursosOnlineContext;
                jwtGenerador = jwt;
                passwordHasher = password;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                #region validaciones
                var usuarioId = await userManager.FindByNameAsync(request.UserName);
                if (usuarioId == null)
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new { mensaje = "El usuario no existe" });
                var EmailEnUso = await context.Users.Where(p => p.Email.Equals(request.Email) && p.UserName != request.UserName).AnyAsync();
                if (EmailEnUso)
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.InternalServerError, new { mensaje = "El Email ingresado ya esta en uso" });
                #endregion

                #region modificacion
                usuarioId.NombreCompleto = request.Nombre + " " + request.Apellidos;
                usuarioId.PasswordHash = passwordHasher.HashPassword(usuarioId, request.Password);
                usuarioId.Email = request.Email;
                var rpt = await userManager.UpdateAsync(usuarioId);
                if (rpt.Succeeded) {
                    var listaRoles = new List<string>(await userManager.GetRolesAsync(usuarioId));
                    return new UsuarioData
                    {
                        NombreCompleto = usuarioId.NombreCompleto,
                        Email = usuarioId.Email,
                        UserName = request.UserName,
                        Token = jwtGenerador.CrearToken(usuarioId, listaRoles),
                    };
                }
                else
                    throw new System.Exception("No se pudo modificar el usuario");
                #endregion
            }
        }
    }
}
