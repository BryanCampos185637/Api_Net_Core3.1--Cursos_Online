using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; }
        }
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(p => p.Titulo).NotEmpty();
                RuleFor(p => p.Descripcion).NotEmpty();
                RuleFor(p => p.FechaPublicacion).NotEmpty();
            }
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
                var curso = await context.Curso.FindAsync(request.CursoId);
                if (curso == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { curso = "No se encontro el curso" });
                }
                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;
                curso.FechaCreacion=DateTime.UtcNow;
                //actualizar el precio del curso
                var precioEntidad = context.Precio.Where(p => p.CursoId.Equals(curso.CursoId)).FirstOrDefault();
                if (precioEntidad != null)
                {
                    precioEntidad.Promocion = request.Promocion ?? precioEntidad.Promocion;
                    precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
                }
                else
                {
                    precioEntidad = new Precio
                    {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = curso.CursoId
                    };
                    await context.Precio.AddAsync(precioEntidad);
                }
                //fin agregar precio
                
                //agregar instructores del curso
                if (request.ListaInstructor != null && request.ListaInstructor.Count > 0)
                {
                    //eliminamos los instructores actuales
                    var instructores = context.CursoInstructor.Where(p => p.CursoId == request.CursoId).ToList();
                    //ejecutamos la eliminacion
                    foreach (var objInstructor in instructores)
                    {
                        context.CursoInstructor.Remove(objInstructor);
                    }
                    //fin del procedimiento para eliminar

                    //agregamos los nuevos instructores
                    foreach (var id in request.ListaInstructor)
                    {
                        var nuevoInstructor = new CursoInstructor
                        {
                            CursoId = request.CursoId,
                            InstructorId = id
                        };
                        context.CursoInstructor.Add(nuevoInstructor);
                    }
                }
                var rpt = await context.SaveChangesAsync();
                if (rpt > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo modificar el curso");
            }
        }
    }
}