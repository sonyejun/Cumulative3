using Cumulative3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cumulative3.Controllers
{
    public class TeacherController : Controller
    {
        //We can instantiate the teachercontroller outside of each method
        private TeacherDataController teacherdatacontroller = new TeacherDataController();


        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        //GET : /Teacher/Error
        /// <summary>
        /// This window is for showing Teacher Specific Errors!
        /// </summary>
        public ActionResult Error()
        {
            return View();
        }

        //GET : /Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            try
            {
                //Try to get a list of teachers.
                IEnumerable<Teacher> Teachers = teacherdatacontroller.ListTeachers(SearchKey);
                return View(Teachers);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //Debug.WriteLine(ex.Message);
                return RedirectToAction("Error", "Teacher");
            }
        }

        //GET : /Teacher/Ajax_List
        public ActionResult Ajax_List()
        {
            return View();
        }

        //GET : /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            try
            {
                Teacher Teacher = teacherdatacontroller.FindTeacher(id);
                
                return View(Teacher);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Teacher");
            }
        }

        //GET : /Teacher/Ajax_Show
        public ActionResult Ajax_Show(int id)
        {
            // ViewBag id add
            ViewBag.TeacherId = id;

            return View();
        }




        //GET : /Teacher/Add
        public ActionResult Add()
        {
            return View();
        }
        //GET : /Teacher/Ajax_Add
        public ActionResult Ajax_Add()
        {
            return View();
        }


        //POST : /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, decimal Salary)
        {
            try
            {
                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.Salary = Salary;

                teacherdatacontroller.AddTeacher(NewTeacher);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Teacher");
            }


            return RedirectToAction("List");
        }


        /// <summary>
        /// Routes to a dynamically generated "Teacher Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Teacher</param>
        /// <returns>A dynamic "Update Teacher" webpage which provides the current information of the Teacher and asks the user for new information as part of a form.</returns>
        /// <example>GET : /Teacher/Update/5</example>
        public ActionResult Update(int id)
        {
            try
            {
                Teacher SelectedTeacher = teacherdatacontroller.FindTeacher(id);
                return View(SelectedTeacher);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Teacher");
            }
        }


        //GET : /Teacher/Ajax_Update
        public ActionResult Ajax_Update(int id)
        {
            try
            {
                Teacher SelectedTeacher = teacherdatacontroller.FindTeacher(id);
                return View(SelectedTeacher);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Teacher");
            }
        }



        /// <summary>
        /// Receives a POST request containing information about an existing Teacher in the system, with new values. Conveys this information to the API, and redirects to the "Teacher Show" page of our updated Teacher.
        /// </summary>
        /// <param name="id">Id of the Teacher to update</param>
        /// <param name="TeacherFname">The updated first name of the Teacher</param>
        /// <param name="TeacherLname">The updated last name of the Teacher</param>
        /// <param name="EmployeeNumber">The updated EmployeeNumber of the Teacher.</param>
        /// <param name="Salary">The updated Salary of the Teacher.</param>
        /// <returns>A dynamic webpage which provides the current information of the Teacher.</returns>
        /// <example>
        /// PUT : /Teacher/Update/10
        /// FORM DATA / PUT DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Christine",
        ///	"TeacherLname":"Bittle",
        ///	"EmployeeNumber":"T395",
        ///	"Salary": 54.45
        /// }
        /// </example>
        [HttpPost]
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, string EmployeeNumber, decimal Salary)
        {
            try
            {
                Teacher TeacherInfo = new Teacher();
                TeacherInfo.TeacherFname = TeacherFname;
                TeacherInfo.TeacherLname = TeacherLname;
                TeacherInfo.EmployeeNumber = EmployeeNumber;
                TeacherInfo.Salary = Salary;
                teacherdatacontroller.UpdateTeacher(id, TeacherInfo);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Teacher");
            }


            return RedirectToAction("Show/" + id);
        }

        //GET : /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                Teacher SelectedTeacher = teacherdatacontroller.FindTeacher(id);
                return View(SelectedTeacher);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //GET : /Teacher/Ajax_DeleteConfirm/{id}
        public ActionResult Ajax_DeleteConfirm(int id)
        {
            try
            {
                Teacher SelectedTeacher = teacherdatacontroller.FindTeacher(id);
                return View(SelectedTeacher);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //POST : /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                teacherdatacontroller.DeleteTeacher(id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }


    }
}