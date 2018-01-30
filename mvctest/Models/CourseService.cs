using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public class CourseService : ICourseService
    {

        private ICourseRepository Repository;
        private IValidationDictonary Validator;
        
        public CourseService(IValidationDictonary dictonary, ICourseRepository repository)
        {
            Validator = dictonary;
            Repository = repository;
        }

        //get a list of all courses from the database
        public List<Course> ListAllCourses()
        {
            List<Course> ret;
            // Database logic
            ret = Repository.ListAllCourses();
            return ret;
        }

        //validate and create a course
        public bool CreateCourse(Course courseToCreate)
        {
            if (ValidateCourse(courseToCreate))
            {
                return Repository.CreateCourse(courseToCreate);//create course
            }
            else
            {
                return false;//pass up invalid flag
            }
        }

        //get a course by id (for editing, without enrollment list)
        public Course GetCourseByID(int id)
        {
            return Repository.GetCourseByID(id);
        }

        //validate and edit an existing course
        public bool EditCourse(Course courseToEdit)
        {
            if (ValidateCourse(courseToEdit))
            {
                return Repository.EditCourse(courseToEdit);//create course
            }
            else
            {
                return false;//pass up invalid flag
            }
        }

        public Course GetCourseCompleteByID(int id)
        {
            return Repository.GetCourseCompleteByID(id);
        }
        public List<User> GetNotEnrolled(int courseID)
        {
            return Repository.GetNotEnrolled(courseID);
        }
        public bool AdjustEnrollment(int courseID, List<int> enrolled)
        {
            return Repository.AdjustEnrollment(courseID, enrolled);
        }
        public List<User> ListAllUsers()
        {
            return Repository.ListAllUsers();
        }
        public List<Term> ListAllTerms()
        {
            return Repository.ListAllTerms();
        }



        //validation
        protected bool ValidateCourse(Course courseToValidate)
        {
            if (String.IsNullOrWhiteSpace(courseToValidate.Title))//general requirements not null
                Validator.AddError("Creating.Title", "Title is required");
            if (String.IsNullOrWhiteSpace(courseToValidate.courseCode))
                Validator.AddError("Creating.CourseCode", "Course code is required");

            if (courseToValidate.DayOfWeek < 0 || courseToValidate.DayOfWeek > 7)//bounds validation for day of week and time
                Validator.AddError("DayOfWeek", "Day of week is out of range");
            if (courseToValidate.TimeOfDay < 0 || courseToValidate.TimeOfDay > 23)//time
                Validator.AddError("TimeOfDay", "Time of day is out of range");
            if (courseToValidate.Term == null)
                Validator.AddError("Term", "Please specify a term");
                    
            return Validator.IsValid;
        }
    }
}