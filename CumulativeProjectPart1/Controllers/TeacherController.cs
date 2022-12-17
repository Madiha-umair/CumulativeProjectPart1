using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CumulativeProject.Models;

namespace CumulativeProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        //GET : Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.TeacherInformation(SearchKey);
            return View(Teachers);
        }

        //GET : Teacher/Show/{id}
        public ActionResult Show(int id)
        {

            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);
        }


        //GET : Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {

            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);
        }

        //POST: /Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //GET : /Teacher/New

        public ActionResult New()
        {
            return View();
        }
        /// <summary>
        /// Receives a POST request containing information about teacher to be added in the system,
        /// with new values. Conveys this information to the API, and redirects to the "Teacher List"
        /// page of our add teacher.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="TeacherFname"></param>
        /// <param name="TeacherLname"></param>
        /// <param name="HireDate"></param>
        /// <param name="Salary"></param>
        /// <returns>A dynamic webpage which provides the list of the teacher.</returns>
        /// <example>
        /// POST : Teacher/create/10
        /// FROM DATA /POST DATA / REQUEST BODY
        /// {
        /// "TeacherFname": "Madiha";
        /// "TeacherLname":   '"Umair";
        /// "HireDate": "2/01/2022";
        /// "Salary": "4000";
        /// }
        /// </example>

        //POST : /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, DateTime HireDate, decimal Salary)
        {
            //Identify that this method is running
            //Identify the inputs provided from the form

            Debug.WriteLine("I have access to Create Method");
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(HireDate);
            Debug.WriteLine(Salary);

            Teacher NewTeacher = new Teacher();
            if(TeacherFname =="")
            { Response.Write("<script>alert('User first name should be required!');</script>"); }
            NewTeacher.TeacherFname= TeacherFname;
            NewTeacher.TeacherLname= TeacherLname;
            NewTeacher.HireDate= HireDate;
            NewTeacher.Salary= Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }

        //GET: /Teacher/Edit/{id}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            //Need to get the information about the teacher
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);
        }

        /// <summary>
        /// Receives a POST request containing information about an existing teacher in the system,
        /// with new values. Conveys this information to the API, and redirects to the "Teacher Show"
        /// page of our updated teacher.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="TeacherFname"></param>
        /// <param name="TeacherLname"></param>
        /// <param name="HireDate"></param>
        /// <param name="Salary"></param>
        /// <returns>A dynamic webpage which provides the current information of the teacher.</returns>
        /// <example>
        /// POST : Teacher/Update/10
        /// FROM DATA /POST DATA / REQUEST BODY
        /// {
        /// "TeacherFname": "Madiha";
        /// "TeacherLname":   '"Umair";
        /// "HireDate": "2/01/2022";
        /// "Salary": "4000";
        /// }
        /// </example>
        // POST: /Teacher/Update/{id}
        [HttpPost]
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, DateTime HireDate, Decimal Salary)
        {
            //Need to recieving the information about the teacher

            Debug.WriteLine("Recieving information about teacher");
            Debug.WriteLine(id);
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(HireDate);
            Debug.WriteLine(Salary);

            Teacher UpdatedTeacher = new Teacher();
            //Use C# Server Side Validation to ensure that there is no missing information when a teacher is updated(such as teacher Salary)
            UpdatedTeacher.TeacherFname = TeacherFname;
            UpdatedTeacher.TeacherLname = TeacherLname;
            UpdatedTeacher.HireDate = HireDate;
            UpdatedTeacher.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, UpdatedTeacher);

            return RedirectToAction("Show/" +id);

        }


        //GET : /Teacher/New

        public ActionResult Ajax_NewTeacher()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNew(string TeacherFname, string TeacherLname, DateTime HireDate, decimal Salary)
        {
            //Identify that this method is running
            //Identify the inputs provided from the form

            Debug.WriteLine("I have access to Create Method");
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(HireDate);
            Debug.WriteLine(Salary);

            Teacher AddNewTeacher = new Teacher();
            AddNewTeacher.TeacherFname= TeacherFname;
            AddNewTeacher.TeacherLname= TeacherLname;
            AddNewTeacher.HireDate= HireDate;
            AddNewTeacher.Salary= Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(AddNewTeacher);

            return RedirectToAction("List");
        }
    }
}