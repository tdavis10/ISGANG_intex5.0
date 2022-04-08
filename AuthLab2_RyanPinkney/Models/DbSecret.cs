using System;
// Authors Jacob Poor, Ryan Pinkney, Kevin Gutierrez, Tanner Davis
namespace AuthLab2_RyanPinkney.Models
{
    public class DbSecret
    {
        public static string GetRDSConnectionString(string dbname = "ebdb")
        {
            // used to call the environmental variables
            if (string.IsNullOrEmpty(dbname)) return null;
            string server = Environment.GetEnvironmentVariable("RDS_SERVER");
            string port = Environment.GetEnvironmentVariable("RDS_PORT");
            string user = Environment.GetEnvironmentVariable("RDS_USER");
            string password = Environment.GetEnvironmentVariable("RDS_PASSWORD");
            string thing = Environment.GetEnvironmentVariable("CONNECTIONSTRING");
            return thing;
        }
    }

}
