using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public interface IInstructor
    {
        Task<IEnumerable<InstructorModel>> obtenerLista();
        Task<InstructorModel> obtenerPorId(Guid id);
        Task<int> Nuevo(string Nombre, string Apellidos, string Grado);
        Task<int> Actualiza(Guid InstructorId, string Nombre, string Apellidos, string Grado);
        Task<int> Eliminar(Guid id);
    }
}
