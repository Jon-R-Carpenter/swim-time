using System.Collections.Generic;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public class TermService : ITermService
    {
        private ITermRepository Repository;
        private IValidationDictonary Validator;

        public TermService(IValidationDictonary dictonary, ITermRepository repository)
        {
            Validator = dictonary;
            Repository = repository;
        }

        //get a list of all terms from the database
        public List<Term> ListAllTerms()
        {
            List<Term> ret;
            // Database logic
            ret = Repository.ListAllTerms();
            return ret;
        }

        //validate and create a course
        public bool CreateTerm(Term termToCreate)
        {
            if (ValidateTerm(termToCreate))
            {
                termToCreate.Title = termToCreate.Quarter + " " + termToCreate.Year;
                return Repository.CreateTerm(termToCreate);//create term
            }
            else
            {
                return false;//pass up invalid flag
            }
        }

        public bool EditTerm(Term termToEdit)
        {
            if (ValidateTerm(termToEdit))
            {
                termToEdit.Title = termToEdit.Quarter + " " + termToEdit.Year;
                return Repository.EditTerm(termToEdit);//create term
            }
            else
            {
                return false;//pass up invalid flag
            }
        }

        //validation
        protected bool ValidateTerm(Term termToValidate)
        {
            return true;
        }
    }
}