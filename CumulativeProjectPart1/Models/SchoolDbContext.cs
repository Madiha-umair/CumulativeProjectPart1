using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace CumulativeProject.Models
{
    public class SchoolDbContext
    {
        //These are readonly "secret" properties.
        //only SchooldbContext class can use them.
        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "schooldbf2022"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }
        
      //ConnectionString is a series of credentials used to connect to the database.  
        private static string ConnectionString
        {
            get 
            {
                return "server = " + Server
                    + "; user = " + User
                    + "; database = " +Database
                    + "; port = " + Port
                    + "; password = " + Password;
            }
        }
        //This is the method actually use to get the database!
        /// <summary>
        /// Returns a connection to the schooldbf2022 database.
        /// </summary>
        /// <example>
        /// private SchooldbContext School = new SchooldbContext();
        /// MySqlConnection Conn = School.AccessDatabase();
        /// </example>
        /// <returns>A Mysqlconnection Objects</returns>
        public MySqlConnection AccessDatabase()
        {
            // Instantiating the MySqlConnection Class to creat an object
            //This Object is a specific connection to our schooldbf2022 database on port 3306 of localhost
            return new MySqlConnection(ConnectionString); 
        }
        
}
}