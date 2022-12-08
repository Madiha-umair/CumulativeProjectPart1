using CumulativeProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CumulativeProject.Controllers
{
    public class TeacherDataController : ApiController
    {
        //The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        //This controller will access the teachers table of our schooldbf2022 database.
        /// <summary>
        /// Returns a list of teachers in the system
        /// </summary>
        /// <example>
        /// GET api/TeacherData/TeacherInformation
        /// </example>
        /// <returns>
        /// A list of teachers (first names and last names)
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/TeacherInformation/{SearchKey?}")]
        //[EnableCors(origins:"*", methods:"*", headers:"*")]
        public IEnumerable<Teacher> TeacherInformation(string SearchKey=null)
        {
            //Creat an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and data base
            Conn.Open();

           //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower( @key)";
            cmd.Parameters.AddWithValue("@key", "%" +SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher>{};
            
            //Loop Through each row the Result Set
            while (ResultSet.Read())
            {
                //Access column innformation by the DB column name as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];

                Teacher NewTeacher=new Teacher();
                NewTeacher.TeacherId=TeacherId;
                NewTeacher.TeacherFname=TeacherFname;
                NewTeacher.TeacherLname=TeacherLname;
                NewTeacher.HireDate=HireDate;
                NewTeacher.Salary=Salary;


                //Add NewTeacher (object), fields (information) to the list
                Teachers.Add(NewTeacher);
            }
            //Close the connection between the MySQL Daabase and the WebServer
            Conn.Close();

            //Return the final list of teacher names
            return Teachers ;  
        }
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Creat an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and data base
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "Select * from teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();  

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Loop Through each row the Result Set
            while (ResultSet.Read())
            {
                //Access column innformation by the DB column name as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];

                //Assigning values to the fields of Newteacher object.
                NewTeacher.TeacherId=TeacherId;
                NewTeacher.TeacherFname=TeacherFname;
                NewTeacher.TeacherLname=TeacherLname;
                NewTeacher.HireDate=HireDate;
                NewTeacher.Salary=Salary;
            }

            //Returns the fields/information of teacher selected by the user from the list of teachers.
            return NewTeacher;
        }

        /// <summary>
        /// Delete a teacher from the system
        /// </summary>
        /// <param name="id"></param>
        /// <example>POST : /api/TeacherData/DeleteTeacher/7</example>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Creat an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and data base
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "Delete from teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }
        
        /// <summary>
        /// Add an article into the system
        /// </summary>
        /// <param name="NewTeacher"></param>
        /// <returns></returns>
        
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody]Teacher NewTeacher)
        {

            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and data base
            Conn.Open();

           
            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, hiredate, salary) values (@TeacherFname, @TeacherLname, @HireDate, @Salary)";
            cmd.Parameters.AddWithValue("@TeacherFname",NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@Hiredate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// Updates a teacher  on the MySQL database. Non-Deterministic.
        /// </summary>
        /// <param name="TeacherId">THe id of the teacher in the system</param>
        /// <param name="UpdateTeacher">post content, all teacher attributes</param>
        /// <example>POST : /api/TeacherData/UpdateTeacher/7
        /// POST CONTENT / FORM BODY/ REQUEST BODY
        /// {"teacherfname":"Christine", "teacherlname":"Bittle", "hiredate":"","salary":"6000"}
        /// </example>
        [HttpPost]
        [Route("api/TeacherData/UpdateTeacher/{TeacherId}")]
        // public void UpdateTeacher(Teacher UpdateTeacher) // pass the whole object of type teacher
        // public void UpdateTeacher(int teacherId, string teacherfname,......all parameters)
        public void UpdateTeacher(int TeacherId, [FromBody]Teacher UpdatedTeacher)
        {

            Debug.WriteLine("updating information about teacher having id" + TeacherId);
            Debug.WriteLine("POST CONTENT");
            Debug.WriteLine(UpdatedTeacher.TeacherFname);
            Debug.WriteLine(UpdatedTeacher.TeacherLname);
            Debug.WriteLine(UpdatedTeacher.HireDate);
            Debug.WriteLine(UpdatedTeacher.Salary);

            string query = "update teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname,  hiredate=@HireDate, salary=@Salary where teacherid=@TeacherId";

            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and data base
            Conn.Open();


            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
            //SQL Query

            cmd.Parameters.AddWithValue("@TeacherId",TeacherId);
            cmd.Parameters.AddWithValue("@TeacherFname", UpdatedTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", UpdatedTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@Hiredate", UpdatedTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", UpdatedTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

    }
}
