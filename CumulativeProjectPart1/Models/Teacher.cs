using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CumulativeProject.Models
{
    public class Teacher
    {
        //The following fields define a Teacher
        public int TeacherId;
        public string TeacherFname;
        public string TeacherLname;
        public DateTime HireDate;
        public decimal Salary;


        //parameterless constructor function
        public Teacher() { }
    }

}