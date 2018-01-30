using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimtimeSite.Models.ViewModels      
{
    public class CreateCommentViewModel
    {
        public Comment Creating { get; set; }
        public IEnumerable<SelectListItem> Staff { get; set; }

        //constructors
        public CreateCommentViewModel()
        {
        }

        public CreateCommentViewModel(Comment create, List<User> staff)
        {
            Staff = staff.Select(x => new SelectListItem
            {
                Text = x.LastName + " " + x.FirstName,
                Value = x.UserID.ToString()
            });

            Creating = create;
        }
    }
}