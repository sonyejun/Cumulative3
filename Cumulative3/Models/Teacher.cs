using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;

namespace Cumulative3.Models
{
    public class Teacher
    {
        //The following fields define an Author
        public int TeacherId;
        public string TeacherFname;
        public string TeacherLname;
        public string EmployeeNumber;
        public DateTime Hiredate;
        public decimal Salary;

        public bool IsValid()
        {
            bool valid = true;

            if (TeacherFname == null || TeacherLname == null || EmployeeNumber == null || Salary <= 0)
            {
                //Base validation to check if the fields are entered.
                valid = false;
            }
            else
            {
                //Validation for fields to make sure they meet server constraints
                if (TeacherFname.Length < 2 || TeacherFname.Length > 255) valid = false;
                if (TeacherLname.Length < 2 || TeacherLname.Length > 255) valid = false;
                if (EmployeeNumber.Length < 2 || EmployeeNumber.Length > 255) valid = false;
                Regex RegexSalary = new Regex(@"^\d+(\.\d{1,2})?$");
                if (!RegexSalary.IsMatch(Salary.ToString())) valid = false;
            }
            Debug.WriteLine("The model validity is : " + valid);

            return valid;
        }

        //parameter-less constructor function
        public Teacher() { }
    }
}