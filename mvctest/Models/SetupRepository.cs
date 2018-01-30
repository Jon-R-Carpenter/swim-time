using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSwimTimeSite.Models;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public class SetupRepository : ISetupRepository
    {
        private ThePoolDBService Service = new ThePoolDBService();


        public void UpdateActivation(int[] userIDs, bool status)
        {
            Service.UpdateActivation(userIDs, status);
        }
        public void UpdateStaff(int[] userIDs, bool status)
        {
            Service.UpdateStaff(userIDs, status);
        }
        public void UpdateCertifications(int[] userIDs, int certID, bool add, int staff)
        {
            Service.UpdateCertifications(userIDs, certID, add, staff);
        }
        public void UpdateUserType(int[] userIDs, String type, int staffID)
        {
            Service.UpdateUserType(userIDs, type, staffID);
        }
        public void UpdateUserClass(int[] userIDs, String classInSchool, int staff)
        {
            Service.UpdateUserClass(userIDs, classInSchool, staff);
        }
        public List<Equipment> ListAllEquipment()
        {
            return Service.ListAllEquipment();
        }
        public List<Certification> ListAllCertifications()
        {
            return Service.ListAllCertifications();
        }
        public List<VisitType> ListAllVisitTypes()
        {
            return Service.ListAllVisitTypes();
        }
        public List<Comment> ListAllComments()
        {
            return Service.ListAllComments();
        }
        public List<String> GetUserTypes()
        {
            return Service.GetUserTypes();
        }
        public List<User> ListAllUsers()
        {
            return Service.ListAllUsers();
        }
        public List<User> ListUsersFiltered(String filter, String search)
        {
            return Service.ListUsersFiltered(filter, search);
        }
        public List<User> ListStaff()
        {
            return Service.ListStaff();
        }
        public List<User> ListAllUsersComplete()
        {
            return Service.ListAllUsersComplete();
        }
        public bool UpdateComment(int id, bool show)
        {
            return Service.UpdateComment(id, show);
        }
        public bool CreateEquipment(Equipment creating)
        {
            return Service.CreateEquipment(creating);
        }
        public bool CreateVisitType(VisitType creating)
        {
            return Service.CreateVisitType(creating);
        }
        public bool CreateCertification(Certification creating)
        {
            return Service.CreateCertification(creating);
        }
        public bool EditEquipment(Equipment editing)
        {
            return Service.EditEquipment(editing);
        }
        public Equipment GetEquipmentByID(int id)
        {
            return Service.GetEquipmentByID(id);
        }
        public VisitType GetVisitTypeByID(int id)
        {
            return Service.GetVisitTypeByID(id);
        }
        public bool EditVisitType(VisitType editing)
        {
            return Service.EditVisitType(editing);
        }
        public Certification GetCertificationByID(int id)
        {
            return Service.GetCertificationByID(id);
        }
        public bool EditCertification(Certification editing)
        {
            return Service.EditCertification(editing);
        }

        public List<Course> ListAllCourses()
        {
            return Service.ListAllCourses();
        }

        public List<SwimTime> ListAllSwimTime()
        {
            return Service.ListAllSwimTimeRules();
        }

        public bool CreateSwimTime(SwimTime create)
        {
            return Service.CreateSwimTimeRule(create);
        }

        public bool EditSwimTime(SwimTime editing)
        {
            return Service.EditSwimTimeRule(editing);
        }

        public SwimTime GetSwimTimeByID(int id)
        {
            return Service.GetSwimTimeByID(id);
        }
        

        public List<Term> ListAllTerms()
        {
            return Service.ListAllTerms();
        }

        public bool CreateTerm(Term create)
        {
            return Service.CreateTerm(create);
        }

        public bool EditTerm(Term editing)
        {
            return Service.EditTerm(editing);
        }

        public Term GetTermByID(int id)
        {
            return Service.GetTermByID(id);
        }
    }
}