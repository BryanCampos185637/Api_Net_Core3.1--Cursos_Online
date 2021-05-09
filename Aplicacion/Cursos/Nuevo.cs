using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid>ListaInstructor { get; set; }
            public decimal Precio { get; set; }
            public decimal Promocion {get; set;}
        }
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion(){
                RuleFor(p=>p.Titulo).NotEmpty();
                RuleFor(p=>p.Descripcion).NotEmpty();
                RuleFor(p=>p.FechaPublicacion).NotEmpty();
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
                Guid cursoId = Guid.NewGuid();
                var curso = new Curso
                {
                    CursoId=cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion,
                    FechaCreacion= DateTime.UtcNow
                };
                context.Curso.Add(curso);
                if (request.ListaInstructor != null)
                {
                    foreach (var id in request.ListaInstructor)
                    {
                        var cursoInstructor = new CursoInstructor
                        {
                            CursoId = cursoId,
                            InstructorId = id
                        };
                        context.CursoInstructor.Add(cursoInstructor);
                    }
                }
                //logica para agrear precio del curso
                var precioEntidad = new Precio
                {
                    CursoId=cursoId,
                    PrecioActual= request.Precio,
                    Promocion=request.Promocion,
                    PrecioId= Guid.NewGuid()
                };
                //agregamos el precio
                context.Precio.Add(precioEntidad);

                var rpt = await context.SaveChangesAsync();
                if (rpt > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}