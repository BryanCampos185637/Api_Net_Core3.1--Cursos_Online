using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class RolLista
    {
        public class Ejecuta : IRequest<List<IdentityRole>> { }
        public class Manejador : IRequestHandler<Ejecuta, List<IdentityRole>>
        {
            private readonly CursosOnlineContext context;
            public Manejador(CursosOnlineContext onlineContext)
            {
                context = onlineContext;
            }
            public async Task<List<IdentityRole>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var lista = await context.Roles.ToListAsync();
                if (lista != null)
                    return lista;
                else
                    throw new Exception("No se pudo completar la transacción");
            }
        }
    }
}
