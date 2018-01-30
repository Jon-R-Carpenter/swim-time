using System;
using System.Collections.Generic;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public class SetupService : ISetupService
    {
        private ISetupRepository Repository;
        private IValidationDictonary Validator;
        
        public SetupService(IValidationDictonary dictonary, ISetupRepository repository)
        {
            Validator = dictonary;
            Repository = repository;
        }

        public void UpdateActivation(int[] userIDs, bool status)
        {
            Repository.UpdateActivation(userIDs, status);
        }
        public void UpdateStaff(int[] userIDs, bool status)
        {
            Repository.UpdateStaff(userIDs, status);
        }
        public void UpdateCertifications(int[] userIDs, int certID, bool add, int staff)
        {
            Repository.UpdateCertifications(userIDs, certID, add, staff);
        }
        public void UpdateUserType(int[] userIDs, String type, int staffID)
        {
            Repository.UpdateUserType(userIDs, type, staffID);
        }
        public void UpdateUserClass(int[] userIDs, String classInSchool, int staff)
        {
            Repository.UpdateUserClass(userIDs, classInSchool, staff);
        }
        public List<Equipment> ListAllEquipment()
        {
            return Repository.ListAllEquipment();
        }
        public List<Certification> ListAllCertifications()
        {
            return Repository.ListAllCertifications();
        }
        public List<VisitType> ListAllVisitTypes()
        {
            return Repository.ListAllVisitTypes();
        }
        public List<Comment> ListAllComments()
        {
            return Repository.ListAllComments();
        }
        public List<User> ListAllUsers()
        {
            return Repository.ListAllUsers();
        }
        public List<User> ListUsersFiltered(String filter, String search)
        {
            return Repository.ListUsersFiltered(filter, search);
        }
        public List<User> ListStaff()
        {
            return Repository.ListStaff();
        }
        public List<String> GetUserTypes()
        {
            return Repository.GetUserTypes();
        }
        public List<User> ListAllUsersComplete()
        {
            return Repository.ListAllUsersComplete();
        }
        public bool UpdateComment(int id, bool show)
        {
            return Repository.UpdateComment(id, show);
        }
        public bool CreateEquipment(Equipment creating)
        {
            if (ValidateEquipment(creating))
            {
                return Repository.CreateEquipment(creating);
            }
            else
            {
                return false;
            }

        }
        public bool CreateVisitType(VisitType creating)
        {
            if (ValidateVisitType(creating))
            {
                return Repository.CreateVisitType(creating);
            }
            else
            {
                return false;
            }
        }
        public VisitType GetVisitTypeByID(int id)
        {
            return Repository.GetVisitTypeByID(id);
        }
        public bool EditVisitType(VisitType editing)
        {
            if (ValidateVisitType(editing))
            {
                return Repository.EditVisitType(editing);
            }
            else
            {
                return false;
            }
        }

        public bool CreateCertification(Certification creating)
        {
            if (ValidateCertification(creating))
            {
                return Repository.CreateCertification(creating);
            }
            else
            {
                return false;
            }
        }

        public Certification GetCertificationByID(int id)
        {
            return Repository.GetCertificationByID(id);
        }
        public bool EditCertification(Certification editing)
        {
            if (ValidateCertification(editing))
            {
                return Repository.EditCertification(editing);
            }
            else
            {
                return false;
            }
        }

        public Equipment GetEquipmentByID(int id)
        {
            return Repository.GetEquipmentByID(id);
        }
        public bool EditEquipment(Equipment editing)
        {
            if (ValidateEquipment(editing))
            {
                return Repository.EditEquipment(editing);
            }
            else
            {
                return false;
            }
        }
        public List<Course> ListAllCourses()
        {
            List<Course> ret;
            // Database logic
            ret = Repository.ListAllCourses();
            return ret;
        }

        public List<SwimTime> ListAllSwimTime()
        {
            List<SwimTime> ret;
            // Database logic
            ret = Repository.ListAllSwimTime();
            return ret;
        }

        public bool CreateSwimTime(SwimTime create)
        {
            //VALIDATEION HERE USE VALIDATION SPACE BELOW!! model it off those methods and return false if it doesn't work
            if (ValidateSwimTime(create))
            {
                return Repository.CreateSwimTime(create);
            }
            else
            {
                return false;
            }
        }

        public bool EditSwimTime(SwimTime editing)
        {
            if (ValidateSwimTime(editing))
            {
                return Repository.EditSwimTime(editing);
            }
            else
            {
                return false;
            }
        }

        public SwimTime GetSwimTimeByID(int id)
        {
            return Repository.GetSwimTimeByID(id);
        }

        public List<Term> ListAllTerms()
        {
            List<Term> ret;
            // Database logic
            ret = Repository.ListAllTerms();
            return ret;
        }

        public bool CreateTerm(Term create)
        {
            //VALIDATEION HERE USE VALIDATION SPACE BELOW!! model it off those methods and return false if it doesn't work
            if (ValidateTerm(create))
            {
                create.Title = create.Quarter + " " + create.Year;
                return Repository.CreateTerm(create);
            }
            else
            {
                return false;
            }
        }

        public bool EditTerm(Term editing)
        {
            if (ValidateTerm(editing))
            {
                editing.Title = editing.Quarter + " " + editing.Year;
                return Repository.EditTerm(editing);
            }
            else
            {
                return false;
            }
        }

        public Term GetTermByID(int id)
        {
            return Repository.GetTermByID(id);
        }

        #region Validation

        private bool ValidateCertification(Certification validating)
        {
            //if (String.IsNullOrWhiteSpace(validating.Title))
                //Validator.AddError("Title", "You must enter a title");

            return Validator.IsValid;
        }

        private bool ValidateVisitType(VisitType validating)
        {
            if (String.IsNullOrWhiteSpace(validating.Title))
                Validator.AddError("Title", "You must enter a title");
           

            return Validator.IsValid;
        }

        private bool ValidateEquipment(Equipment validating)
        {
            if (String.IsNullOrWhiteSpace(validating.Name))
                Validator.AddError("Name", "You must enter a name");
            if (String.IsNullOrWhiteSpace(validating.Description))
                Validator.AddError("Description", "You must enter a description");

            return Validator.IsValid;

        }

        private bool ValidateSwimTime(SwimTime validating)
        {
            if(validating.MinHoursPerVisit < 0)
                Validator.AddError("Creating.MinHoursPerVisit", "Min Hours must be greater than 0");
            if (validating.MaxHoursPerVisit < 0)
                Validator.AddError("Creating.MaxHoursPerVisit", "Max Hours must be greater than 0");
            if (validating.MinHoursPerVisit > validating.MaxHoursPerVisit)
                Validator.AddError("Creating.MaxHoursPerVisit", "Max Hours must be larger than Min Hours");
            if (validating.Start < new DateTime(2010, 01, 01))
                Validator.AddError("Creating.Start", "Start Date must be after 01/01/2010");
            if (validating.Start > validating.End)
                Validator.AddError("Creating.End", "The End Date must be after the Start Date");
            
            return Validator.IsValid;
        }

        private bool ValidateTerm(Term validating)
        {
            int num;
            if (int.TryParse(validating.Year, out num))
            {
                if (num < 2011)
                    Validator.AddError("Creating.Year", "Year must be after 2010");
            }

            if (num != validating.Start.Year)
                Validator.AddError("Creating.Start", "The Start Date must be the same year as Year");

            if (validating.Start > validating.Endd)
                Validator.AddError("Creating.End", "The End Date must be after the Start Date");

            if (String.IsNullOrEmpty(validating.Quarter))
                Validator.AddError("Creating.Quarter", "You must specify a quarter");
            
            return Validator.IsValid;
        }

        #endregion

    }
}