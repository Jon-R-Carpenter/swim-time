using System.Collections.Generic;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public interface ITermService
    {
        List<Term> ListAllTerms();
        bool CreateTerm(Term termToCreate);
        bool EditTerm(Term termToEdit);
    }
}