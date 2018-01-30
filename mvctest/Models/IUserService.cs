using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheSwimTimeSite.Models.ViewModels;

namespace TheSwimTimeSite.ObjectModels
{
    public interface IUserService
    {
        int CreateUser(CreateEditUserViewModel creating);
        List<String> GetUserTypes(); 
        User GetUserByID(int id);
        User GetUserForCheckout(int id);
        bool EditUser(CreateEditUserViewModel editing);
        User GetCompleteUserByID(int id);
        List<Certification> ListAvailableCerts(int id);
        bool AddCertifications(int userID, List<int> adding, int staffID);
        List<User> SearchUsersByLastname(String lastname);
        List<User> SearchBySimilarLastNames(String lastname);
        User SearchUsersByCardscan(String cardscan);
        List<VisitType> ListAvailableVisitTypes(int id);
        List<Equipment> ListEquipment();        
        int SubmitCourseVisit(int userID, int courseID, bool checkout, string staffUser,string ip);
        int SubmitCheckout(int userID, bool valid, string staffuser, string ip, int associatedVisit);
        int SubmitGeneralVisit(int userID, int visitTypeID, string staffUser, string ip);
        bool SubmitEquipment(int[] equip, int visitID);
        List<User> ListStaff();
        bool UserExistsByName(String lastname, String firstname);
        List<User> ListUsersIn();
        List<SwimTime> GetRulesByCourse(int courseID);
        List<Visit> ListUserVisitsForCheckout(int userID, int courseID);
    }
}
