using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;



namespace TheSwimTimeSite.ObjectModels
{
    public class Term
    {
        [ScaffoldColumn(false)]
        public int TermID { get; set; }
        //[Required]
        public String Quarter { get; set; }
        //[Required(ErrorMessage="The Year field must be after 2010")]
        //[StringLength(4,MinimumLength=4, ErrorMessage="Year must be 4 numbers")]
        //[RegularExpression("([0-9]{4})", ErrorMessage = "Year must be 4 numbers and after 2010")]
        public String Year { get; set; }
        public DateTime Start { get; set; }
        public DateTime Endd { get; set; }
        public String Title { get; set; }

      
       
        public String QuarterString()
        {
            return this.Quarter;
        }

        public Term()
        {
        }

        public Term(int termID, String Quarter, String Year, DateTime Start, DateTime End)
        {
            this.TermID = termID;
            this.Quarter = Quarter;
            this.Year = Year;
            this.Start = Start;
            this.Endd = End;
        }

        public Term(int termID, String Quarter, String Year, DateTime Start, DateTime End, String title)
        {
            this.TermID = termID;
            this.Quarter = Quarter;
            this.Year = Year;
            this.Start = Start;
            this.Endd = End;
            this.Title = title;
        }
    }
}