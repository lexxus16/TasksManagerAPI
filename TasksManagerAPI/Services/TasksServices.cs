// TasksServices.cs

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TasksManagerAPI.Models;
using Microsoft.Extensions.Configuration;

namespace TasksManagerAPI.Services
{
    public class TasksServices : ITasksServices
    {
        private readonly string _connectionString = "Server=DESKTOP-01SOFCO\\SQLEXPRESS; Database=StudentDB; Integrated Security=True; TrustServerCertificate=True;";

        // Constructor accepts IConfiguration to get the connection string from appsettings.json
        //public TasksServices(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetConnectionString("DefaultConnection");
        //}

        // Get all tasks
        public IEnumerable<TasksModel> GetAllTasks()
        {
            var tasks = new List<TasksModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Title, Description, DueDate, Priority, Status, UserID FROM Tasks";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var task = new TasksModel
                            {
                                ID = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                DueDate = reader.GetDateTime(3),
                                Priority = reader.GetString(4),
                                Status = reader.GetString(5),
                                UserID = reader.GetInt32(6)
                            };
                            tasks.Add(task);
                        }
                    }
                }
            }
            return tasks;
        }

        // Get task by ID
        public TasksModel GetTaskById(int id)
        {
            TasksModel task = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Title, Description, DueDate, Priority, Status, UserID FROM Tasks WHERE ID = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = id });

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            task = new TasksModel
                            {
                                ID = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                DueDate = reader.GetDateTime(3),
                                Priority = reader.GetString(4),
                                Status = reader.GetString(5),
                                UserID = reader.GetInt32(6)
                            };
                        }
                    }
                }
            }

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }

            return task;
        }

        // Add a new task
        public TasksModel AddTask(TasksModel task)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Tasks (Title, Description, DueDate, Priority, Status, UserID) " +
                               "OUTPUT INSERTED.ID " +
                               "VALUES (@Title, @Description, @DueDate, @Priority, @Status, @UserID)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Title", SqlDbType.VarChar) { Value = task.Title });
                    command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar) { Value = task.Description });
                    command.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.DateTime) { Value = task.DueDate });
                    command.Parameters.Add(new SqlParameter("@Priority", SqlDbType.VarChar) { Value = task.Priority });
                    command.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar) { Value = task.Status });
                    command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = task.UserID });

                    task.ID = (int)command.ExecuteScalar(); // Get the ID of the newly inserted task
                }
            }

            return task;
        }

        // Update an existing task
        public TasksModel UpdateTask(int id, TasksModel task)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Tasks SET Title = @Title, Description = @Description, DueDate = @DueDate, " +
                               "Priority = @Priority, Status = @Status, UserID = @UserID WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = id });
                    command.Parameters.Add(new SqlParameter("@Title", SqlDbType.VarChar) { Value = task.Title });
                    command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar) { Value = task.Description });
                    command.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.DateTime) { Value = task.DueDate });
                    command.Parameters.Add(new SqlParameter("@Priority", SqlDbType.VarChar) { Value = task.Priority });
                    command.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar) { Value = task.Status });
                    command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = task.UserID });

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new KeyNotFoundException($"Task with ID {id} not found.");
                    }
                }
            }

            return task;
        }

        // Delete a task
        public void DeleteTask(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Tasks WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = id });

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new KeyNotFoundException($"Task with ID {id} not found.");
                    }
                }
            }
        }
    }
}
