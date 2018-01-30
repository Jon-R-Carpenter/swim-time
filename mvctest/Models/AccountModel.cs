using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using TheSwimTimeSite.ObjectModels;

namespace mvctest.Models
{
    public class AccountModel
    {
        private List<Account> listAccounts = new List<Account>();
        MembershipUserCollection users = Membership.GetAllUsers();
      

            

    public AccountModel()
        {
            string type = "";
            foreach (MembershipUser mu in users)
            {
                StaffUser user = new StaffUser(mu.UserName, mu.LastLoginDate, mu.IsApproved);
                user.IsAdmin = Roles.IsUserInRole(mu.UserName, "Administrators");
                user.IsStaff = Roles.IsUserInRole(mu.UserName, "Staff");
                if (user.IsAdmin)
                {
                    type = "Admin";
                }
                else if (user.IsStaff)
                {
                    type = "Staff";
                }

                listAccounts.Add(new Account { userName = mu.UserName, Password = "123", Roles = new string[] {type} });
              
            }

        }
        public Account find(String userName)
        {
            return listAccounts.Where(acc => acc.userName.Equals(userName)).FirstOrDefault();
        }
        public Account login(string userName, string password)
        {
            return listAccounts.Where(acc => acc.userName.Equals(userName)&&
            acc.Password.Equals(password)).FirstOrDefault();
        }

    }
}