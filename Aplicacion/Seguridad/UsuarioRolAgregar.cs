using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuarioRolAgregar
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
            private readonly UserManager<Usuario> userManager;
            private readonly RoleManager<IdentityRole> roleManager;
            public Manejador(UserManager<Usuario> user, RoleManager<IdentityRole> role)
            {
                userManager = user;
                roleManager = role;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                #region validaciones
                var rol = await roleManager.FindByNameAsync(request.RolName);
                if (rol == null)
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new { mensaje = "Error no existe rol" });
                var usuario = await userManager.FindByNameAsync(request.UserName);
                if (usuario == null)
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new { mensaje = "Error no existe usuario" });
                #endregion

                #region agregamos la relacion
                var rpt = await userManager.AddToRoleAsync(usuario, request.RolName);
                if (rpt.Succeeded)
                    return Unit.Value;
                else
                    throw new System.Exception("No se pudo asignar el rol al usuario");
                #endregion
            }
        }
    }
}
