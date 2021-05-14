using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuarioRoleEliminar
    {
        public class Ejecuta : IRequest
        {
            public string UserName { get; set; }
            public string RolName { get; set; }
        }
        public class EjecutaValidator : AbstractValidator<Ejecuta>
        {
            public EjecutaValidator()
            {
                RuleFor(p => p.UserName).NotEmpty();
                RuleFor(p => p.RolName).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly UserManager<Usuario> userManager;
            public Manejador(RoleManager<IdentityRole> _roleManager, UserManager<Usuario> user)
            {
                roleManager = _roleManager;
                userManager = user;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                #region validaciones
                var usuario = await userManager.FindByNameAsync(request.UserName);
                if (usuario == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "Error no existe usuario" });
                var rol = await roleManager.FindByNameAsync(request.RolName);
                if (rol == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "Error no existe rol" });
                #endregion
                #region eliminar rol
                var rpt = await userManager.RemoveFromRoleAsync(usuario, request.RolName);
                if (rpt.Succeeded)
                    return Unit.Value;
                else
                    throw new Exception("No se pudo eliminar el rol");
                #endregion
            }
        }
    }
}
