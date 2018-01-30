using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public class HomeRepository : IHomeRepository
    {
        private ThePoolDBService Service = new ThePoolDBService();

        public List<User> ListUsersIn()
        {
            return Service.ListUsersIn();
        }

        public List<Comment> ListComments()
        {
            return Service.ListComments();
        }

        public List<User> ListStaff()
        {
            return Service.ListStaff();
        }
        /*
        public bool CreateComment(Comment creating)
        {
            return Service.CreateComment(creating);
        }
        */
    }
}