using System;
using System.Collections.Generic;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public interface ISetupService
    {
        List<Equipment> ListAllEquipment();
        List<Certification> ListAllCertifications();
        List<VisitType> ListAllVisitTypes();
        List<Comment> ListAllComments();
        List<User> ListAllUsers();
        List<User> ListUsersFiltered(String filter, String search);
        List<User> ListAllUsersComplete();
        List<String> GetUserTypes();
        List<User> ListStaff();

        void UpdateActivation(int[] userIDs, bool status);
        void UpdateStaff(int[] userIDs, bool status);
        void UpdateCertifications(int[] userIDs, int certID, bool add, int staff);
        void UpdateUserType(int[] userIDs, String type, int staff);
        void UpdateUserClass(int[] userIDs, String classInSchool, int staff);

        bool UpdateComment(int id, bool show);
        bool CreateEquipment(Equipment creating);
        bool CreateVisitType(VisitType creating);
        bool CreateCertification(Certification creating);
        bool EditEquipment(Equipment editing);
        Equipment GetEquipmentByID(int id);
        VisitType GetVisitTypeByID(int id);
        bool EditVisitType(VisitType editing);
        Certification GetCertificationByID(int id);
        bool EditCertification(Certification editing);
        List<Course> ListAllCourses();
        List<SwimTime> ListAllSwimTime();
        bool CreateSwimTime(SwimTime create);
        bool EditSwimTime(SwimTime editing);
        SwimTime GetSwimTimeByID(int id);
        List<Term> ListAllTerms();
        bool CreateTerm(Term create);
        bool EditTerm(Term editing);
        Term GetTermByID(int id);

    }
}
