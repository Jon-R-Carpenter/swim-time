﻿using System.Collections.Generic;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public interface IHomeRepository
    {
        List<User> ListUsersIn();
        List<Comment> ListComments();
       // bool CreateComment(Comment creating);
        List<User> ListStaff();
    }
}
