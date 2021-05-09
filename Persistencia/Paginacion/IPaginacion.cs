using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.Paginacion
{
    public interface IPaginacion
    {
        Task<PaginacionModel> develoverPaginacion(string storedProcedure, 
            int numeroPagina,
            int cantidadElementos, 
            IDictionary<string, object> parametrosFiltro,
            string ordenamientoColumna);
    }
}
