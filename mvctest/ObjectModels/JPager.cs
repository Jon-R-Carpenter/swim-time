using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class JPager
    {
        public String Jsonstr { get; set; }
        public int PageSize { get; set; }
        public int ShowNumbers { get; set; }
        public int SortColumn { get; set; }
        public int Direction { get; set; }

        //constructors
        //Jpager with ordering
        public JPager(String json, int pagesize, int shownumbers,
            int sortcolumn, int direction)
        {
            Jsonstr = json;
            PageSize = pagesize;
            ShowNumbers = shownumbers;
            SortColumn = sortcolumn;
            Direction = direction;
        }
        //JPager without ordering
        public JPager(String json, int pagesize, int shownumbers)
        {
            Jsonstr = json;
            PageSize = pagesize;
            ShowNumbers = shownumbers;
        }
        //default constructor
        public JPager()
        {
        }
    }
}