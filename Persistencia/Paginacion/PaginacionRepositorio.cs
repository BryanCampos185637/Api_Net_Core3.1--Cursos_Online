using Dapper;
using Persistencia.DapperConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace Persistencia.Paginacion
{
    public class PaginacionRepositorio : IPaginacion
    {
        private readonly IFactoryConnection factoryConnection;
        public PaginacionRepositorio(IFactoryConnection factory)
        {
            factoryConnection = factory;
        }
        public async Task<PaginacionModel> develoverPaginacion(string storedProcedure, int numeroPagina, 
            int cantidadElementos, IDictionary<string, object> parametrosFiltro, string ordenamientoColumna)
        {
            PaginacionModel paginacionModel = new PaginacionModel();
            List<IDictionary<string, object>> listaReporte = null;
            int totalRecords = 0, totalPaginas = 0;
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                foreach(var param in parametrosFiltro)
                {
                    parameters.Add("@" + param.Key, param.Value);
                }
                //parametro de entrada
                parameters.Add("@NumeroPagina", numeroPagina);
                parameters.Add("@CantidadElemento", cantidadElementos);
                parameters.Add("@Ordenamiento", ordenamientoColumna);
                //parametro de salida
                parameters.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                parameters.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);
                var con = factoryConnection.GetConnection();//abrimos la conexion
                var result = await con.QueryAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                //convertimos a IDictoniary
                listaReporte = result.Select(p => (IDictionary<string, object>)p).ToList();
                paginacionModel.ListaRecord = listaReporte;
                paginacionModel.NumeroPaginas = parameters.Get<int>("@TotalPaginas");
                paginacionModel.TotalRecords = parameters.Get<int>("@TotalRecords");
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo ejecutar la paginacion " + e.Message, e);
            }
            finally 
            {
                factoryConnection.CloseConnection();
            }
            return paginacionModel;
        }
    }
}
