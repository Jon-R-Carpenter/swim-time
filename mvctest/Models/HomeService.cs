using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public class HomeService : IHomeService
    {
        private IHomeRepository Repository;
        private IValidationDictonary Validator;
        
        public HomeService(IValidationDictonary dictonary, IHomeRepository repository)
        {
            Validator = dictonary;
            Repository = repository;
        }
        
        //methods

        public List<User> ListUsersIn()
        {
            return Repository.ListUsersIn();
        }

        public List<Comment> ListComments()
        {
            return Repository.ListComments();
        }
        public List<User> ListStaff()
        {
            return Repository.ListStaff();
        }
        /*
        public bool CreateComment(Comment creating)
        {
            if (ValidateComment(creating))
                return Repository.CreateComment(creating);

            return false;
        }
        */
        //validation
        protected bool ValidateComment(Comment validating)
        {
            if(String.IsNullOrWhiteSpace(validating.Title))
                Validator.AddError("Creating.Title", "You must have a title");
            if(String.IsNullOrWhiteSpace(validating.Text))
                Validator.AddError("Creating.Text", "You must have a body");
            if(validating.Staff == -1)//no staff selected
                Validator.AddError("Creating.Staff", "You must select staff to certify");

            return Validator.IsValid;
        }
    }
}