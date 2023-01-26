using Microsoft.Data.SqlClient;
using System;
using System.Text;

namespace Vilain_Names
{
    public class Startup
    {
        private const string ConnectionString = @"Server=DESKTOP-VLH0QE3\SQLEXPRESS03;Database=MinionsDB;Trusted_Connection=True;";
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

              string result = getMinionsInfo(sqlConnection);
              Console.WriteLine(result);
        }

        private static string getMinionsInfo(SqlConnection sqlConnection)
        {
            string villainMoreThan3MinionsQueryText = @"SELECT v.[Name] AS VillainName, COUNT(mv.VillainId) AS Count
                                                        FROM Villains AS v
                                                        JOIN MinionsVillains AS mv
                                                        ON v.Id = mv.VillainId
                                                        JOIN Minions AS m
                                                        ON m.Id = mv.MinionId
                                                        GROUP BY v.Id, v.Name
                                                        HAVING COUNT(mv.VillainId) > 3
                                                        ORDER BY COUNT(mv.VillainId)";

            using SqlCommand getVillianMoreThan3Minions = new SqlCommand(villainMoreThan3MinionsQueryText, sqlConnection);

            using SqlDataReader reader = getVillianMoreThan3Minions.ExecuteReader();

            StringBuilder sb = new StringBuilder();

            while (reader.Read())
            {
                string villainName = reader["VillainName"].ToString();
                string minionsCount = reader["Count"].ToString();
                sb.AppendLine($"{villainName} - {minionsCount}");
            }
            return sb.ToString().TrimEnd(); 
        }
    }
}
