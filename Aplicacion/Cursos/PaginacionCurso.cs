using MediatR;
using Persistencia.Paginacion;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class PaginacionCurso
    {
        public class Ejecuta : IRequest<PaginacionModel>
        {
            public string Titulo { get; set; }
            public int NumeroPagina { get; set; }
            public int CantidadElemento { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecuta, PaginacionModel>
        {
            private readonly IPaginacion paginacion;
            public Manejador(IPaginacion _paginacion)
            {
                paginacion = _paginacion;
            }
            public async Task<PaginacionModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var storedProcedure = "sp_obtener_curso_paginacion";
                var ordenamiento = "Titulo";
                var parametros = new Dictionary<string, object>();
                parametros.Add("NombreCurso", request.Titulo);
                return await paginacion.develoverPaginacion(storedProcedure, request.NumeroPagina, request.CantidadElemento, parametros, ordenamiento);
            }
        }
    }
}
