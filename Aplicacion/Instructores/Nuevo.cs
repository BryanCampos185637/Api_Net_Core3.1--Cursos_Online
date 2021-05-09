using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Nuevo
    {
        public class Ejecuta : IRequest 
        {
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Titulo { get; set; }
        }
        public class EjecutaValida: AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(p => p.Nombre).NotEmpty();
                RuleFor(p => p.Titulo).NotEmpty();
                RuleFor(p => p.Apellidos).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor instructor;
            public Manejador(IInstructor _instructor)
            {
                instructor = _instructor;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var rpt = await instructor.Nuevo(request.Nombre, request.Apellidos, request.Titulo);
                if (rpt > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el instructor");
            }
        }
    }
}
