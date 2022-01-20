using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

#region 
/*  User Manual
    1. Выход exit
 */
#endregion


namespace StudentsApp1
{
    internal class Program
    {
        private static string connectionString = 
            ConfigurationManager.ConnectionStrings["Students1DB"].ConnectionString;
        
        private static SqlConnection sqlConnection =null;

        static void Main(string[] args)
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            Console.WriteLine("**StudentsApp**\n");

            SqlDataReader sqlDataReader = null;
            
            string command = string.Empty;

            while (true)
            {
                try
                {
                    Console.Write("> ");
                    command = Console.ReadLine();

                    #region Exit
                    if (command.ToLower().Equals("exit"))
                    {
                        if (sqlConnection.State == ConnectionState.Open)
                        {
                            sqlConnection.Close();
                        }

                        if (sqlDataReader != null)
                        {
                            sqlDataReader.Close();
                        }
                        break;
                    }
                    #endregion

                    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

                    switch (command.Split(' ')[0].ToLower())
                    {
                        case "select":

                            sqlDataReader = sqlCommand.ExecuteReader();

                            while (sqlDataReader.Read())
                            {
                                Console.WriteLine($"{sqlDataReader["Id"]} {sqlDataReader["FIO"]} " +
                                    $"{sqlDataReader["Birthday"]} {sqlDataReader["University"]} " +
                                    $"{sqlDataReader["Group_number"]} {sqlDataReader["Course"]} " +
                                    $"{sqlDataReader["Average_Score"]}");

                                Console.WriteLine(new String('-', 30));
                            }

                            if (sqlDataReader != null)
                            {
                                sqlDataReader.Close();
                            }
                            break;
                        case "insert":

                            Console.WriteLine($"Добавлено : {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        case "update":

                            Console.WriteLine($"Изменено : {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        case "delete":

                            Console.WriteLine($"Удалено : {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        default:
                            Console.WriteLine($"Команда {command} некорректна!");
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.WriteLine("для продолжения нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
