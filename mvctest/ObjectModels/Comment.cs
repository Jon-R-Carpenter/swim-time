using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int Staff { get; set; }//simple id of staff that created
        public User CreatedBy { get; set; }
        public String Title { get; set; }
        public String Text { get; set; }
        public DateTime DatePosted { get; set; }
        public bool Showing { get; set; }

        //constructors
        public Comment()
        {

        }

        public Comment(int id, String title, DateTime posted, String text)
        {
            CommentID = id;
            Title = title;
            DatePosted = posted;
            Text = text;
        }

        public Comment(int id, String title, DateTime posted, String text, bool showing)
        {
            CommentID = id;
            Title = title;
            DatePosted = posted;
            Text = text;
            Showing = showing;
        }
    }
}