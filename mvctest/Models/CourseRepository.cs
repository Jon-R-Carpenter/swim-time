using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public class CourseRepository : ICourseRepository
    {
        private ThePoolDBService Service = new ThePoolDBService();


        public List<Course> ListAllCourses()
        {
            return Service.ListAllCourses();
        }
        public bool CreateCourse(Course courseToCreate)
        {
            return Service.CreateCourse(courseToCreate);
        }
        public bool EditCourse(Course courseToEdit)
        {
            return Service.EditCourse(courseToEdit);
        }
        public Course GetCourseByID(int id)
        {
            return Service.GetCourseByID(id);
        }
        public Course GetCourseCompleteByID(int id)
        {
            return Service.GetCourseCompleteByID(id);
        }
        public List<User> GetNotEnrolled(int courseID)
        {
            return Service.GetNotEnrolled(courseID);
        }
        public List<User> ListAllUsers()
        {
            return Service.ListAllUsers();
        }
        public bool AdjustEnrollment(int courseID, List<int> enrolled)
        {
            return Service.AdjustEnrollment(courseID, enrolled);
        }
        public List<Term> ListAllTerms()
        {
            return Service.ListAllTerms();
        }

    }
}