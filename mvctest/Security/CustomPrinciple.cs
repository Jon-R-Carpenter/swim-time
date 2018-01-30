using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using mvctest.Models;

namespace mvctest.Security
{
    public class CustomPrinciple : IPrincipal
    {
        private Account Account;
        private AccountModel am = new AccountModel();
        
        public CustomPrinciple(Account account)
        {
            this.Account = account;
            this.Identity = new GenericIdentity(account.userName);
        }
        public IIdentity Identity
        {
            get;
            set;
            
        }

        public bool IsInRole(string role)
        {
            var roles = role.Split(new char[] {','});
            return roles.Any(r => this.Account.Roles.Contains(r));
        }
    }
}