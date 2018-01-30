using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Comment> Comments { get; set; }
        public List<User> UsersIn { get; set; }

        public HomeViewModel(List<Comment> comments, List<User> usersIn)
        {
            Comments = comments;
            UsersIn = usersIn;
        }
    }
}