using MediatR;
using Persistencia.DapperConexion.Instructor;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Consulta 
    {
        public class Lista : IRequest<List<InstructorModel>> { }

        public class Manejador : IRequestHandler<Lista, List<InstructorModel>>
        {
            private readonly IInstructor instructor;
            public Manejador(IInstructor _instructor)
            {
                instructor = _instructor;
            }
            public async Task<List<InstructorModel>> Handle(Lista request, CancellationToken cancellationToken)
            {
                var resultado = await instructor.obtenerLista();
                return resultado.ToList();
            }
        }
    }
}
