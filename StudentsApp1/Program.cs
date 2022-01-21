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
    1. Выход - exit
    2. Вывод всех записей - selectall
    3. Очистка экрана - clear
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

                    #region Clear
                    if (command.ToLower().Equals("clear"))
                    {
                        Console.Clear();
                        continue;
                    }
                    #endregion

                    SqlCommand sqlCommand = null;

                    string[] commandArray = command.ToLower().Split(' ');//массив строк, на который "разрезана" строка команды по пробелу

                    switch (commandArray[0])
                    {
                        case "selectall":
                            sqlCommand = new SqlCommand("SELECT * FROM [Students]", sqlConnection);

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
                        case "select":
                            sqlCommand = new SqlCommand(command, sqlConnection);

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
                            sqlCommand = new SqlCommand(command, sqlConnection);
                            Console.WriteLine($"Добавлено : {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        case "update":
                            sqlCommand = new SqlCommand(command, sqlConnection);
                            Console.WriteLine($"Изменено : {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        case "delete":
                            sqlCommand = new SqlCommand(command, sqlConnection);
                            Console.WriteLine($"Удалено : {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        case "sortby":

                            //sortby fio asc - по возрастанию
                            sqlCommand = new SqlCommand(
                                $"SELECT * FROM [Students] ORDER BY {commandArray[1]} {commandArray[2]}", sqlConnection);

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
