using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient; // только этот, убрать System.Data.SqlClient
using TaskManagerDesktop.Models;

namespace TaskManagerDesktop.Data
{
    public static class DatabaseHelper
    {
        public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            var dataTable = new DataTable();
            try
            {
                using var connection = DatabaseConnection.GetConnection();
                using var command = new SqlCommand(query, connection);
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                using var adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка выполнения запроса: {ex.Message}", ex);
            }
            return dataTable;
        }

        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            try
            {
                using var connection = DatabaseConnection.GetConnection();
                using var command = new SqlCommand(query, connection);
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка выполнения команды: {ex.Message}", ex);
            }
        }

        public static object? ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            try
            {
                using var connection = DatabaseConnection.GetConnection();
                using var command = new SqlCommand(query, connection);
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка выполнения скалярного запроса: {ex.Message}", ex);
            }
        }

        public static List<Priority> GetPriorities()
        {
            var priorities = new List<Priority>();
            var dataTable = ExecuteQuery("SELECT PriorityId, Name, Level, Color FROM Priorities ORDER BY Level");
            foreach (DataRow row in dataTable.Rows)
            {
                priorities.Add(new Priority
                {
                    Id = Convert.ToInt32(row["PriorityId"]),
                    Name = row["Name"].ToString() ?? "",
                    Level = Convert.ToInt32(row["Level"]),
                    Color = row["Color"].ToString() ?? "#808080"
                });
            }
            return priorities;
        }

        public static List<Category> GetCategories()
        {
            var categories = new List<Category>();
            var dataTable = ExecuteQuery("SELECT CategoryId, Name, Icon FROM Categories");
            foreach (DataRow row in dataTable.Rows)
            {
                categories.Add(new Category
                {
                    Id = Convert.ToInt32(row["CategoryId"]),
                    Name = row["Name"].ToString() ?? "",
                    Icon = row["Icon"]?.ToString() ?? ""
                });
            }
            return categories;
        }
    }
}
