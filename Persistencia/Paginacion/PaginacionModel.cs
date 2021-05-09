using System.Collections.Generic;

namespace Persistencia.Paginacion
{
    public class PaginacionModel
    {
        public List<IDictionary<string, object>> ListaRecord { get; set; }
        public int TotalRecords { get; set; }
        public int NumeroPaginas { get; set; }
    }
}
