using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSwimTimeSite.ObjectModels;
using TheSwimTimeSite.Models;

namespace TheSwimTimeSite.Models
{
    public class SwimTimeService : ISwimTimeService
    {
        private ISwimTimeRepository Repository;
        private IValidationDictonary Validator;

        public SwimTimeService(IValidationDictonary dictonary, ISwimTimeRepository repository)
        {
            Validator = dictonary;
            Repository = repository;
        }

        //get a list of all terms from the database
        public List<SwimTime> ListAllSwimTimeRules()
        {
            List<SwimTime> ret;
            // Database logic
            ret = Repository.ListAllSwimTimeRules();
            return ret;
        }

        //validate and create a course
        public bool CreateSwimTimeRule(SwimTime swimTimeRulesToCreate)
        {
            if (ValidateTerm(swimTimeRulesToCreate))
            {
                return Repository.CreateSwimTimeRule(swimTimeRulesToCreate);//create term
            }
            else
            {
                return false;//pass up invalid flag
            }
        }

        public bool EditSwimTimeRule(SwimTime swimTimeRulesToEdit)
        {
            if (ValidateTerm(swimTimeRulesToEdit))
            {
                return Repository.EditSwimTimeRule(swimTimeRulesToEdit);//create term
            }
            else
            {
                return false;//pass up invalid flag
            }
        }

        public List<Course> ListAllCourses()
        {
            return Repository.ListAllCourses();
            
        }

        //validation
        protected bool ValidateTerm(SwimTime swimTimeRuleToValidate)
        {
            return true;
        }
    }
}