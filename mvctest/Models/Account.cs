using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace mvctest.Models
{
    public class Account
    {
        [Display(Name="Username")]
        public String userName { get; set; }
        [ScaffoldColumn(false)]
        public String Password { get; set; }
        public String[] Roles { get; set; }

    }
}