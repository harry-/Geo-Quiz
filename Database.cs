using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace GeoQuiz
{
    internal class Database
    {
        private static readonly SqlConnection Connection = new SqlConnection("Data Source = SR19-01EDV\\" + "SERVERHARRY; Integrated Security = False; User ID = geoquiz; Password=geheimespasswort; Database = geoquiz; Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite");
        private static SqlDataReader _reader = null;

        internal static User SearchByUsername(string text)
        {
            try
            {
                Connection.Open();
                var username = new SqlParameter
                {
                    Value = text,
                    ParameterName = "@USERNAME"
                };

                var command = new SqlCommand
                {
                    Connection = Connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SEARCH_USER"
                };

                command.Parameters.Add(username);
                Debug.WriteLine(command.CommandText);
                _reader = command.ExecuteReader();

                //if (_reader.RecordsAffected == -1)
                //    return null;

                while (_reader.Read())
                {
                    Debug.WriteLine("user found");
                    var user = new User
                    {
                        Name = _reader[0].ToString(),
                        Hash = _reader[1].ToString()
                    };

                    return user;
                }
                return null;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.ToString());
                Debug.WriteLine(e.Number);
                throw;
            }
            finally
            {
                Connection?.Close();
                _reader?.Close();
            }
        }

        internal static User CreateUser(string username, string password)
        {
            var user = new User
            {
                Name = username,
                Password = password
            };
            user.CreateHash();

            try
            {
                Connection.Open();

                var param1 = new SqlParameter
                {
                    ParameterName = "@USERNAME",
                    SqlDbType = SqlDbType.VarChar,
                    Value = username,
                    Direction = ParameterDirection.Input
                };

                var param2 = new SqlParameter
                {
                    ParameterName = "@PASSWORD",
                    SqlDbType = SqlDbType.VarChar,
                    Value = user.Hash,
                    Direction = ParameterDirection.Input
                };

                var command = new SqlCommand
                {
                    Connection = Connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "CREATE_USER"
                };

                command.Parameters.Add(param1);
                command.Parameters.Add(param2);

                int queryResult = command.ExecuteNonQuery();
                if (queryResult < 0)
                {
                    // query result is always -1 ??? 
                }
                return user;

            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
                if (e.ErrorCode == -2146232060)
                {
                    // this happens if the user already exists in the db
                    return null;
                }

                throw;
            }
            finally
            {
                Connection?.Close();
                _reader?.Close();
            }
        }

        internal static int[] GetTotalScore(string user)
        {
            int[] score = { 0, 0 };
            try
            {
                Connection.Open();
                var username = new SqlParameter
                {
                    Value = user,
                    ParameterName = "@USERNAME"
                };

                var command = new SqlCommand
                {
                    Connection = Connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "TOTAL_SCORE"
                };

                command.Parameters.Add(username);
                Debug.WriteLine(command.CommandText);
                _reader = command.ExecuteReader();
                while (_reader.Read())
                {
                    score[0] = Convert.ToInt32( _reader[0]);
                    score[1] = Convert.ToInt32(_reader[1]);
                }
                return score;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
                return score;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return score;
            }
            finally
            {
                Connection?.Close();
                _reader?.Close();
            }
        }

        internal static string HallOfFame()
        {
            var output = "";
            try
            {
                Connection.Open();

                var command = new SqlCommand
                {
                    Connection = Connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT username, SUM(questions) AS total, SUM(correct) AS correct, SUM(correct) * 100 / SUM(questions) AS percentage FROM dbo.scoresheet GROUP BY username HAVING        (SUM(questions) > 0) ORDER BY percentage DESC"
                };

                _reader = command.ExecuteReader();

                output += "Player\tCorrect\tTotal\tPercentage\n\n";

                while (_reader.Read())
                {
                    output += _reader[0].ToString() + '\t' + _reader[1].ToString() + '\t' + _reader[2] + '\t' + _reader[3] + "%\n";
                }

            return output;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
                return output;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return output;
            }
            finally
            {
                Connection?.Close();
                _reader?.Close();
            }
        }

        public static bool CreateGame(string username, int questions, int correct)
        {
            try
            {
                Connection.Open();

                var user = new SqlParameter
                {
                    ParameterName = "@USERNAME",
                    SqlDbType = SqlDbType.VarChar,
                    Value = username,
                    Direction = ParameterDirection.Input
                };

                var totalQuestions = new SqlParameter
                {
                    ParameterName = "@QUESTIONS",
                    SqlDbType = SqlDbType.Int,
                    Value = questions,
                    Direction = ParameterDirection.Input
                };

                var correctAnswers = new SqlParameter
                {
                    ParameterName = "@CORRECT",
                    SqlDbType = SqlDbType.Int,
                    Value = correct,
                    Direction = ParameterDirection.Input
                };

                var command = new SqlCommand
                {
                    Connection = Connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "ADD_GAME"
                };

                command.Parameters.Add(user);
                command.Parameters.Add(totalQuestions);
                command.Parameters.Add(correctAnswers);

                int queryResult = command.ExecuteNonQuery();
                if (queryResult >= 0) return true;
                Debug.WriteLine($"error during storing game data to db (code {queryResult})");
                return false;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Connection?.Close();
                _reader?.Close();
            }
        }
    }
}
