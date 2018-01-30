using System;
using System.Collections.Generic;
using TheSwimTimeSite.ObjectModels;
using TheSwimTimeSite.Models;
using TheSwimTimeSite.Models.ViewModels;

namespace TheSwimTimeSite.ObjectModels
{
    public class UserService : IUserService
    {
        private IUserRepository Repository;
        private IValidationDictonary Validator;
        
        public UserService(IValidationDictonary dictonary, IUserRepository repository)
        {
            Validator = dictonary;
            Repository = repository;
        }

        //create a user and insert user information to the database
        public int CreateUser(CreateEditUserViewModel creating)
        {
            int ret = -999;
            // Validation logic
            if (!ValidateUser(creating))
                return ret;

            // Database logic
            try
            {
                User staff = Repository.GetUserByID(creating.StaffID);
                String creator = staff.FirstName + " " + staff.LastName;
                ret = Repository.CreateUser(creating.Editing, creator);
            }
            catch
            {
                return -999;
            }
            return ret;
        }

        //get user information by userID
        public User GetUserByID(int id)
        {
            User ret;
            // Database logic
            ret = Repository.GetUserByID(id);
            return ret;
        }
        public User GetUserForCheckout(int id)
        {
            return Repository.GetUserForCheckout(id);
        }
        public bool UserExistsByName(String lastname, String firstname)
        {
            return Repository.UserExistsByName(lastname, firstname);
        }
        public List<User> ListStaff()
        {
            return Repository.ListStaff();
        }
        public List<User> SearchUsersByLastname(String lastname)
        {
            return Repository.SearchUsersByLastname(lastname);
        }
        public List<User> SearchBySimilarLastNames(String lastname)
        {
            return Repository.SearchBySimilarLastNames(lastname);
        }
        public User SearchUsersByCardscan(String cardscan)
        {
            return Repository.SearchUsersByCardscan(cardscan);
        }
        public User GetCompleteUserByID(int id)
        {
            return Repository.GetCompleteUserByID(id);
        }
        public List<String> GetUserTypes()
        {
            return Repository.GetUserTypes();
        }
        public List<User> ListUsersIn()
        {
            return Repository.ListUsersIn();
        }
        public List<Certification> ListAvailableCerts(int id)
        {
            return Repository.ListAvailableCerts(id);
        }
        public bool AddCertifications(int userID, List<int> adding, int staffID)
        {
            if (!ValidateCertify(staffID))//check to see that a staff was selected
                return false;

            return Repository.AddCertifications(userID, adding, staffID);
        }
        public List<VisitType> ListAvailableVisitTypes(int id)
        {
            return Repository.ListAvailableVisitTypes(id);
        }
        public List<Equipment> ListEquipment()
        {
            return Repository.ListEquipment();
        }
        public int SubmitCourseVisit(int userID, int courseID, bool checkout, string staffUser, string ip)        
        {
            return Repository.SubmitCourseVisit(userID, courseID, checkout,staffUser,ip);
        }
        public int SubmitCheckout(int userID, bool valid, string staffUser, string ip, int associatedVisit)
        {
            return Repository.SubmitCheckout(userID, valid, staffUser, ip, associatedVisit); 
        }
        public int SubmitGeneralVisit(int userID, int visitTypeID, string staffUser, string ip)
        {
            return Repository.SubmitGeneralVisit(userID, visitTypeID,staffUser,ip);
        }
        public bool SubmitEquipment(int[] equip, int visitID)
        {
            return Repository.SubmitEquipment(equip, visitID);
        }
        public List<SwimTime> GetRulesByCourse(int courseID)
        {
            return Repository.GetRulesByCourse(courseID);
        }
        public List<Visit> ListUserVisitsForCheckout(int userID, int courseID)
        {
            return Repository.ListUserVisitsForCheckout(userID, courseID);
        }


        //edit user by ID
        public bool EditUser(CreateEditUserViewModel editing)
        {
            // Validation logic
            if (!ValidateUser(editing))
                return false;

            // Database logic
            try
            {
                User staff = Repository.GetUserByID(editing.StaffID);
                String creator = staff.FirstName + " " + staff.LastName;
                Repository.EditUser(editing.Editing, creator);
            }
            catch
            {
                return false;
            }
            return true;
        }

        //validation
        protected bool ValidateUser(CreateEditUserViewModel validate)
        {
            if (String.IsNullOrWhiteSpace(validate.Editing.FirstName))
                Validator.AddError("Editing.FirstName", "First name is required");
            if (String.IsNullOrWhiteSpace(validate.Editing.LastName))
                Validator.AddError("Editing.LastName", "Last name is required");
            if (String.IsNullOrWhiteSpace(validate.Editing.UserType))
                Validator.AddError("Editing.UserType", "User type is required");
            if (validate.Editing.UserType != null && validate.Editing.UserType.ToLower().Equals("student") && String.IsNullOrWhiteSpace(validate.Editing.ClassInSchool))
                Validator.AddError("Editing.ClassInSchool", "You must select a class in school for students");
            if (validate.StaffID == 0)
                Validator.AddError("Editing.StaffID", "Staff Creator is required");
            
            return Validator.IsValid;
        }

        protected bool ValidateCertify(int staffID)
        {
            if(staffID == -1)//no staff selected
                Validator.AddError("Staff", "You must select staff to certify");

            return Validator.IsValid;
        }
    }
}