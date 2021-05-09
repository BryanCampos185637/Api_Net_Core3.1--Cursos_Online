using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Data;

namespace Persistencia.DapperConexion
{
    public class FactoryConnection : IFactoryConnection
    {
        private IDbConnection connection;
        private readonly IOptions<ConexionConfiguracion> configs;
        public FactoryConnection(IOptions<ConexionConfiguracion> _confirgs)
        {
            configs = _confirgs;
        }
        public void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open) 
            {
                connection.Close();
            }
        }

        public IDbConnection GetConnection()
        {
            if (connection == null)
            {
                connection = new SqlConnection(configs.Value.Default);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
            }
            return connection;
        }
    }
}
