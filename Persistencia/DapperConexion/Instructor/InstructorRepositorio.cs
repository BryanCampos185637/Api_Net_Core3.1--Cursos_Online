using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConnection factoryConnection;
        public InstructorRepositorio(IFactoryConnection _factoryConnection)
        {
            factoryConnection = _factoryConnection;
        }
        public async Task<int> Actualiza(Guid InstructorId, string Nombre, string Apellidos, string Grado)
        {
            int rpt = 0;
            try
            {
                var conection = factoryConnection.GetConnection();
                var storedProcedure = "sp_Instructor_Editar";
                rpt = await conection.ExecuteAsync(storedProcedure, new
                {
                    InstructorId = InstructorId,
                    Nombre = Nombre,
                    Apellidos = Apellidos,
                    Grado = Grado
                }, commandType: CommandType.StoredProcedure);
                factoryConnection.CloseConnection();
                return rpt;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo editar el instructor", e);
            }
        }

        public async Task<int> Eliminar(Guid id)
        {
            int rpt = 0;
            try
            {
                var conection = factoryConnection.GetConnection();
                var storedProcedure = "sp_Instructor_Eliminar";
                rpt = await conection.ExecuteAsync(storedProcedure, new
                {
                    InstructorId = id
                }, commandType: CommandType.StoredProcedure);
                factoryConnection.CloseConnection();
                return rpt;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo eliminar el instructor", e);
            }
        }

        public async Task<int> Nuevo(string Nombre, string Apellidos, string Grado)
        {
            int rpt = 0;
            try
            {
                var conection = factoryConnection.GetConnection();
                var storedProcedure = "sp_Instructor_Nuevo";
                rpt = await conection.ExecuteAsync(storedProcedure, new
                {
                    InstructorId = Guid.NewGuid(),
                    Nombre = Nombre,
                    Apellidos = Apellidos,
                    Grado = Grado
                }, commandType: CommandType.StoredProcedure); 
                factoryConnection.CloseConnection();
                return rpt;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo guardar el nuevo instructor", e);
            }
        }

        public async Task<IEnumerable<InstructorModel>> obtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;
            var storedProcedure = "sp_Obtener_Instructores";
            try
            {
                var connection = factoryConnection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storedProcedure, null, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error en la consulta de datos", e);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
            return instructorList;
        }

        public async Task<InstructorModel> obtenerPorId(Guid id)
        {
            InstructorModel instructor = null;
            try
            {
                var conection = factoryConnection.GetConnection();
                var storedProcedure = "sp_Instructor_Obtener_Por_Id";
                instructor = await conection.QueryFirstAsync<InstructorModel>(storedProcedure, new
                {
                    InstructorId = id
                }, commandType: CommandType.StoredProcedure);
                factoryConnection.CloseConnection();
                return instructor;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo obtener el instructor", e);
            }
        }
    }
}
