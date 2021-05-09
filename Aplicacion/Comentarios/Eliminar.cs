using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Comentarios
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext context;
            public Manejador(CursosOnlineContext cursosOnlineContext)
            {
                context = cursosOnlineContext;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var comentario = await context.Comentario.FindAsync(request.Id);
                if (comentario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el comentario" });
                }
                context.Comentario.Remove(comentario);
                var rpt = await context.SaveChangesAsync();
                if (rpt > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo eliminar el comentario");
            }
        }
    }
}