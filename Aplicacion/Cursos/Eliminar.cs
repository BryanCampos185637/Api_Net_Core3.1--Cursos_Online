using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
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
            public Manejador(CursosOnlineContext _context)
            {
                context = _context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //eliminamos primero las referencias
                //eliminamos los instructors
                var instructores = context.CursoInstructor.Where(p => p.InstructorId == request.Id).ToList();
                foreach (var objInstructor in instructores)
                {
                    context.CursoInstructor.Remove(objInstructor);
                }
                //eliminamos los comentarios
                var comentarios = context.Comentario.Where(p => p.CursoId.Equals(request.Id)).ToList();
                foreach (var objComentario in comentarios)
                {
                    context.Comentario.Remove(objComentario);
                }
                //eliminacion de precio
                var precioDB = context.Precio.Where(p => p.CursoId.Equals(request.Id)).FirstOrDefault();
                if (precioDB != null)
                {
                    context.Precio.Remove(precioDB);
                }
                var curso = await context.Curso.FindAsync(request.Id);
                if (curso == null)
                {
                    //throw new Exception("No se encontro ningun curso con el Id "+request.Id.ToString());
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { curso = "No se encontro ningun curso con el Id " + request.Id.ToString() });
                }
                context.Remove(curso);
                var rpt = await context.SaveChangesAsync();
                if (rpt > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudieron guardar los cambios");
            }
        }

    }
}