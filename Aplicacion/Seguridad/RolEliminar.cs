using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class RolEliminar
    {
        public class Ejecuta : IRequest 
        {
            public string Nombre { get; set; }
        }
        public class EjecutaValidator: AbstractValidator<Ejecuta>
        {
            public EjecutaValidator()
            {
                RuleFor(p => p.Nombre).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> roleManager;
            public Manejador(RoleManager<IdentityRole> _roleManager)
            {
                roleManager = _roleManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var rol = await roleManager.FindByNameAsync(request.Nombre);
                if (rol == null)
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, new { mensaje = "No existe el rol" });
                var rpt = await roleManager.DeleteAsync(rol);
                if (rpt.Succeeded)
                    return Unit.Value;
                else
                    throw new Exception("no se pudo eliminar el rol");
            }
        }
    }
}
