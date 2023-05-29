using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;

namespace ApiTransferencias.Common
{
    public class DapperUtils
    {

        // TODO : Mover al appsettings la cadena de conexion
        
        private static readonly string _connectionString = "Data Source=/home/rcampos/Practicas/C#/Curso_Microservicios/ApiTransferencias/transferencias.sqlite";


        internal static IEnumerable<T> Query<T>(string sql, object? param = null)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return connection.Query<T>(sql, param);
            }
        }

        internal static T QueryFirstOrDefault<T>(string sql, object? parameters = null)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<T>(sql, parameters);
            }
        }

        internal static int Execute(string sql, object? param = null)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return connection.Execute(sql, param);
            }
        }

        internal static T ExecuteScalar<T>(string sql, object? parameters = null)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return connection.ExecuteScalar<T>(sql, parameters);
            }
        }

        internal static IEnumerable<T> QueryStoredProcedure<T>(string storedProcedure, object? param = null)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return connection.Query<T>(storedProcedure, param, commandType: CommandType.StoredProcedure);
            }
        }

    }
}



