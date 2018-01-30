using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheSwimTimeSite.ObjectModels
{
    public interface IValidationDictonary
    {
        void AddError(string key, string errorMessage);
        bool IsValid { get; }
    }
}
