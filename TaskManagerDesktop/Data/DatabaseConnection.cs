using System;
using System.Data;
using Microsoft.Data.SqlClient;
using TaskManagerDesktop.Infrastructure;

namespace TaskManagerDesktop.Data
{
    public static class DatabaseConnection
    {
        private static readonly string _connectionString =
            AppConfiguration.GetConnectionString("DefaultConnection");

        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            return connection;
        }

        public static bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                return conn.State == ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }
    }
}
