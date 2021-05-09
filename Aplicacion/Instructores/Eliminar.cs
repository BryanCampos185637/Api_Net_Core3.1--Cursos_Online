using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Eliminar
    {
        public class Ejecuta : IRequest 
        {
            public Guid Id { get; set; }
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
                var rpt = await instructor.Eliminar(request.Id);
                if (rpt > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo eliminar el instructor");
            }
        }
    }
}
