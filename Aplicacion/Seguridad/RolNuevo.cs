using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class RolNuevo
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
        }
        public class validaEjecuta: AbstractValidator<Ejecuta>
            {
            public validaEjecuta()
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
                if (rol != null)
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, new { mensaje = "Ya existe el rol" });
                var rpt = await roleManager.CreateAsync(new IdentityRole(request.Nombre));
                if (rpt.Succeeded)
                    return Unit.Value;
                else
                    throw new Exception("No se pudo guardar el rol"); 
            }
        }
    }
}
