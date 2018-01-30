using System.Collections.Generic;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public interface IHomeService
    {
        List<User> ListUsersIn();
        List<Comment> ListComments();
        //bool CreateComment(Comment creating);
        List<User> ListStaff();
    }
}
