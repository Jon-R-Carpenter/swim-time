using System.Collections.Generic;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public class TermRepository : ITermRepository
    {
        private ThePoolDBService Service = new ThePoolDBService();

        public List<Term> ListAllTerms()
        {
            return Service.ListAllTerms();
        }

        public bool CreateTerm(Term termToCreate)
        {
            return Service.CreateTerm(termToCreate);
        }

        public bool EditTerm(Term termToEdit)
        {
            return Service.EditTerm(termToEdit);
        }
    }
}