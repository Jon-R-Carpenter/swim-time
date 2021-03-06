﻿using System.Collections.Generic;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public interface ICourseService
    {
        List<Course> ListAllCourses();
        bool CreateCourse(Course courseToCreate);
        bool EditCourse(Course courseToEdit);
        Course GetCourseByID(int id);
        Course GetCourseCompleteByID(int id);
        List<User> GetNotEnrolled(int courseID);
        bool AdjustEnrollment(int courseID, List<int> enrolled);
        List<User> ListAllUsers();
        List<Term> ListAllTerms();
    }
}
