using Microsoft.Data.SqlClient;
using System;
using System.Text;

namespace RemoveVillain
{
    public class Startup
    {
        private const string ConnectionString = @"Server=DESKTOP-VLH0QE3\SQLEXPRESS03;Database=MinionsDB;Integrated Security = true;";
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            int villainId = int.Parse(Console.ReadLine());

            string result = RemoveVillainById(sqlConnection, villainId);

            Console.WriteLine(result);
        }

        private static string RemoveVillainById(SqlConnection sqlConnection, int villainId)
        {
            StringBuilder sb = new StringBuilder();
            using SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

            string getVillainNameQueryText = @"SELECT [Name] FROM Villains WHERE Id = @villainId";
            using SqlCommand getVillainNameCmd = new SqlCommand(getVillainNameQueryText, sqlConnection);
            getVillainNameCmd.Parameters.AddWithValue("@villainId", villainId);
            getVillainNameCmd.Transaction = sqlTransaction;

            string villainName = getVillainNameCmd.ExecuteScalar()?.ToString();

            if (villainName == null)
            {
                sb.AppendLine($"No such villain was found.");
            }
            else
            {
                try
                {
                    string releaseMinionsQueryText = @"DELETE FROM MinionsVillains WHERE VillainId = @villainId";
                    using SqlCommand releaseMinionsCmd = new SqlCommand(releaseMinionsQueryText, sqlConnection);
                    releaseMinionsCmd.Parameters.AddWithValue("@villainId", villainId);
                    releaseMinionsCmd.Transaction = sqlTransaction;
                    int releasedMinionsCount = releaseMinionsCmd.ExecuteNonQuery();

                    string deleteVillainQueryText = @"DELETE FROM Villains WHERE Id = @villainId";
                    using SqlCommand deleteVillainCmd = new SqlCommand(deleteVillainQueryText, sqlConnection);
                    deleteVillainCmd.Parameters.AddWithValue("@villainId", villainId);
                    deleteVillainCmd.Transaction = sqlTransaction;
                    deleteVillainCmd.ExecuteNonQuery();

                    sqlTransaction.Commit();

                    sb.AppendLine($"{villainName} was deleted.")
                        .AppendLine($"{releasedMinionsCount} minions were released.");
                }
                catch (Exception ex)
                {
                    sb.AppendLine(ex.Message);

                    try
                    {
                        sqlTransaction.Rollback();
                    }
                    catch (Exception rollbackEx)
                    {
                        sb.AppendLine(rollbackEx.Message);
                    }
                }
            }
            return sb.ToString().TrimEnd();
        }
    }
}