
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Cumulative3.Models;
using MySql.Data.MySqlClient;

namespace Cumulative3.Controllers
{
    public class TeacherDataController : ApiController
    {
        //The database context class which allows us to access our MySQL Database.
        //AccessDatabase switched to a static method, one that can be called without an object.
        MySqlConnection School = SchoolDbContext.AccessDatabase();

        //This Controller Will access the teachers table of our shcool database. Non-Deterministic.
        /// <summary>
        /// Returns a list of teachers in the system
        /// </summary>
        /// <returns>
        /// A list of Teacher Objects with fields mapped to the database column values (first name, last name, employeenumber,hiredate,salary).
        /// </returns>
        /// <example>GET api/TeacherData/ListTeachers -> {Teacher Object, Teacher Object, Teacher Object...}</example>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };
            try
            {
                //Try to open the connection between the web server and database
                School.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = School.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "SELECT * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";

                cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
                cmd.Prepare();

                //Gather Result Set of Query into a variable

                MySqlDataReader ResultSet = cmd.ExecuteReader();

                //Loop Through Each Row the Result Set               
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                    string TeacherFname = ResultSet["teacherfname"].ToString();
                    string TeacherLname = ResultSet["teacherlname"].ToString();
                    string EmployeeNumber = ResultSet["employeenumber"].ToString();
                    DateTime Hiredate = (DateTime)ResultSet["hiredate"];
                    decimal Salary = (decimal)ResultSet["salary"];
        

                    Teacher NewTeacher = new Teacher();
                    NewTeacher.TeacherId = TeacherId;
                    NewTeacher.TeacherFname = TeacherFname;
                    NewTeacher.TeacherLname = TeacherLname;
                    NewTeacher.EmployeeNumber = EmployeeNumber;
                    NewTeacher.Hiredate = Hiredate;
                    NewTeacher.Salary = Salary;

                    //Add the Teacher Name to the List
                    Teachers.Add(NewTeacher);
                }
            }
            catch (MySqlException ex)
            {
                //Catches issues with MySQL.
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                School.Close();

            }

            //Return the final list of teacher names
            return Teachers;

        }

        /// <summary>
        /// Finds an teacher from the MySQL Database through an id. Non-Deterministic.
        /// </summary>
        /// <param name="id">The Teacher ID</param>
        /// <returns>Teacher object containing information about the teacher with a matching ID. Empty Teacher Object if the ID does not match any teachers in the system.</returns>
        /// <example>api/TeacherData/FindTeacher/6 -> {Teacher Object}</example>
        /// <example>api/TeacherData/FindTeacher/10 -> {Teacher Object}</example>
        [HttpGet]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        [Route("api/TeacherData/FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            try
            {
                //Open the connection between the web server and database
                School.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = School.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "Select * from teachers where teacherid = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                //Gather Result Set of Query into a variable
                MySqlDataReader ResultSet = cmd.ExecuteReader();

                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int TeacherId = (int)ResultSet["teacherid"];
                    string TeacherFname = ResultSet["teacherfname"].ToString();
                    string TeacherLname = ResultSet["teacherlname"].ToString();
                    string EmployeeNumber = ResultSet["employeenumber"].ToString();
                    DateTime Hiredate = (DateTime)ResultSet["hiredate"];
                    decimal Salary = (decimal)ResultSet["salary"];

                    NewTeacher.TeacherId = TeacherId;
                    NewTeacher.TeacherFname = TeacherFname;
                    NewTeacher.TeacherLname = TeacherLname;
                    NewTeacher.EmployeeNumber = EmployeeNumber;
                    NewTeacher.Hiredate = Hiredate;
                    NewTeacher.Salary = Salary;


                }
                
                //checking for model validity after pulling from the db
                if (!NewTeacher.IsValid()) throw new HttpResponseException(HttpStatusCode.NotFound);

            }
            catch (HttpResponseException ex)
            {
                Debug.WriteLine(ex);
                throw new ApplicationException("That teacher was not found.", ex);
            }
            catch (MySqlException ex)
            {
                //Catches issues with MySQL.
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                School.Close();

            }


            return NewTeacher;
        }


        // <summary>
        /// Deletes an Teacher from the connected MySQL Database if the ID of that teacher exists. Does NOT maintain relational integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">The ID of the teacher.</param>
        /// <example>Post /api/TeacherData/DeleteTeacher/3</example>
        [HttpPost]
        [Route("api/TeacherData/DeleteTeacher/{id}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void DeleteTeacher(int id)
        {
            try
            {
                //Open the connection between the web server and database
                School.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = School.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "Delete from teachers where teacherid=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                //Catches issues with MySQL.
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                School.Close();

            }
        }

        /// <summary>
        /// Adds an Teacher to the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the teacher's table. </param>
        /// <example>
        /// POST api/TeacherData/AddTeacher
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Christine",
        ///	"TeacherLname":"Bittle",
        ///	"EmployeeNumber":"T395",
        ///	"Salary": 54.45
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        [Route("api/TeacherData/AddTeacher/")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            //Exit method if model fields are not included.
            if (!NewTeacher.IsValid()) throw new ApplicationException("Posted Data was not valid.");
            try
            {
                //Open the connection between the web server and database
                School.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = School.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherFname,@TeacherLname,@EmployeeNumber, CURRENT_DATE(), @Salary)";
                cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
                cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
                cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
                cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                //Catches issues with MySQL.
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                School.Close();

            }

        }

        /// <summary>
        /// Updates an Teacher on the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="TeacherInfo">An object with fields that map to the columns of the teacher's table.</param>
        /// <example>
        /// Post api/TeacherData/UpdateTeacher/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Christine",
        ///	"TeacherLname":"Bittle",
        ///	"EmployeeNumber":"T395",
        ///	"Salary": 54.45
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        [Route("api/TeacherData/UpdateTeacher/{id}")]
        public void UpdateTeacher(int id, [FromBody] Teacher TeacherInfo)
        {


            //Exit method if model fields are not included.
            if (!TeacherInfo.IsValid()) throw new ApplicationException("Posted Data was not valid.");

            try
            {
                //Open the connection between the web server and database
                School.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = School.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "UPDATE teachers SET teacherfname=@TeacherFname, teacherlname=@TeacherLname, employeenumber=@EmployeeNumber, salary=@Salary WHERE teacherid=@TeacherId";
                cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
                cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
                cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
                cmd.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
                cmd.Parameters.AddWithValue("@TeacherId", id);
                cmd.Prepare();
                
                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                //Catches issues with MySQL.
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                School.Close();

            }

        }
    }
}
