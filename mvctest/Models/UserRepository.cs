using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class UserRepository : IUserRepository
    {
        private ThePoolDBService Service = new ThePoolDBService();

        public int CreateUser(User userToCreate, String creator)
        {
            int ret = -999;
            try
            {
                return Service.CreateUser(userToCreate, creator);
            }
            catch
            {
                return ret;
            }
        }

        public User GetUserByID(int id)
        {
            return Service.GetUserByID(id);
        }
        public User GetUserForCheckout(int id)
        {
            return Service.GetUserForCheckout(id);
        }
        public bool UserExistsByName(String lastname, String firstname)
        {
            return Service.UserExistsByName(lastname, firstname);
        }
        public List<User> SearchUsersByLastname(String lastname)
        {
            return Service.SearchUsersByLastname(lastname);
        }
        public User SearchUsersByCardscan(String cardscan)
        {
            return Service.SearchUsersByCardscan(cardscan);
        }
        public List<User> SearchBySimilarLastNames(String lastname)
        {
            return Service.SearchBySimilarLastNames(lastname);
        }
        public User GetCompleteUserByID(int id)
        {
            return Service.GetCompleteUserByID(id);
        }
        public List<User> ListUsersIn()
        {
            return Service.ListUsersIn();
        }
        public List<User> ListStaff()
        {
            return Service.ListStaff();
        }
        public List<Certification> ListAvailableCerts(int id)
        {
            return Service.ListAvailableCerts(id);
        }
        public List<VisitType> ListAvailableVisitTypes(int id)
        {
            return Service.ListAvailableVisitTypes(id);
        }
        public List<Equipment> ListEquipment()
        {
            return Service.ListEquipment();
        }
        public List<String> GetUserTypes()
        {
            return Service.GetUserTypes();
        }
        public bool AddCertifications(int userID, List<int> adding, int staffID)
        {
            return Service.AddCertifications(userID, adding, staffID);
        }
        public int SubmitCourseVisit(int userID, int courseID, bool checkout,string staffUser,string ip)
        {
            return Service.SubmitCourseVisit(userID, courseID, checkout,staffUser,ip);
        }
        public int SubmitCheckout(int userID, bool valid, string staffUser, string ip, int associatedVisit)
        {
            return Service.SubmitCheckout(userID, valid, staffUser, ip, associatedVisit); 
        }
        public int SubmitGeneralVisit(int userID, int visitTypeID, string staffUser, string ip)
        {
            return Service.SubmitGeneralVisit(userID, visitTypeID,staffUser,ip);
        }
        public bool SubmitEquipment(int[] equip, int visitID)
        {
            return Service.SubmitEquipment(equip, visitID);
        }

        public bool EditUser(User user, String creator)
        {
            return Service.EditUser(user, creator);
        }

        public List<User> ListUsers()
        {
            return null;
        }
        public List<SwimTime> GetRulesByCourse(int courseID)
        {
            return Service.GetRulesByCourse(courseID);
        }
        public List<Visit> ListUserVisitsForCheckout(int userID, int courseID)
        {
            return Service.ListUserVisitsForCheckout(userID, courseID);
        }
    }
}