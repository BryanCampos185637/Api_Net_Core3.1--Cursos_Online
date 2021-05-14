using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class ObtenerRolesPorUsuario
    {
        public class Ejecuta : IRequest<List<string>>
        {
            public string UserName { get; set; }
        }
        public class EjecutaValidator : AbstractValidator<Ejecuta>
        {
            public EjecutaValidator()
            {
                RuleFor(p => p.UserName).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta,List<string>>
        {
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly UserManager<Usuario> userManager;
            public Manejador(RoleManager<IdentityRole> _roleManager, UserManager<Usuario> user)
            {
                roleManager = _roleManager;
                userManager = user;
            }

            public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByNameAsync(request.UserName);
                if (user == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "Error no existe usuario" });
                var listaRsult = await userManager.GetRolesAsync(user);
                return new List<string>(listaRsult);
            }
        }
    }
}
