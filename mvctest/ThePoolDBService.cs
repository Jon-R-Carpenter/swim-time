using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite
{
   public class ThePoolDBService
    {
        /*
//Creates a comment
public bool CreateComment(Comment creating)
{
try
{
Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
DbCommand cmd = db.GetStoredProcCommand("Create_Comment");

db.AddInParameter(cmd, "@title_p", DbType.String, creating.Title);
db.AddInParameter(cmd, "@comment_p", DbType.String, creating.Text);
db.AddInParameter(cmd, "@createdBy_p", DbType.Int32, creating.Staff);

db.ExecuteNonQuery(cmd);

//automated email
User staff = this.GetUserByID(creating.Staff);//get creator
String creator = staff.FirstName + " " + staff.LastName;
//ThePoolDBService.EmailMessage(creating.Title, creating.Text, creator);//sent message

}
catch (Exception excp)
{
Exception myExcp = new Exception("Could not add user. Error: " +
excp.Message, excp);
throw (myExcp);
}
return true;

}
*/
        public Report GenerateIndividualReport(Report creating)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Generate_Individual_Report");

                db.AddInParameter(cmd, "@startDate_p", DbType.DateTime, creating.Start);
                db.AddInParameter(cmd, "@endDate_p", DbType.DateTime, creating.Finish.AddDays(1));
                db.AddInParameter(cmd, "@checkouts_p", DbType.Boolean, creating.IncludeOuts);
                db.AddInParameter(cmd, "@firstName_p", DbType.String, creating.First);
                db.AddInParameter(cmd, "@lastName_p", DbType.String, creating.Last);

                DataSet ds = db.ExecuteDataSet(cmd);


                creating.Data.AddRange(from row in ds.Tables[0].AsEnumerable()
                                       select new ReportRow(row.Field<int>("userID"),
                                           row.Field<String>("firstName"),

                                           row.Field<String>("lastName"),

                                           row.Field<String>("studentID"),
                                           row.Field<String>("classInSchool"),
                                           row.Field<String>("userType"),

                                           row.Field<bool>("staffFlag"),

                                           row.Field<DateTime>("dateTimee"),


                                           row.Field<int>("crn")));


            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return creating;

        }

        //Creates a user into the database
        public int CreateUser(User userToCreate, String creator)
        {
            int ret = 0;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Create_User");

                db.AddInParameter(cmd, "@firstName_p", DbType.String, userToCreate.FirstName.TrimEnd(new char[] { ' ' }).TrimStart(new char[] { ' ' }));
                db.AddInParameter(cmd, "@lastName_p", DbType.String, userToCreate.LastName.TrimEnd(new char[] { ' ' }).TrimStart(new char[] { ' ' }));
                db.AddInParameter(cmd, "@sid_p", DbType.String, ThePoolDBService.HashCode(userToCreate.StudentID));
                String encoded = ThePoolDBService.HashCode(userToCreate.CardScan);//encode card scan
                db.AddInParameter(cmd, "@cardscan_p", DbType.String, encoded);

                db.AddInParameter(cmd, "@email_p", DbType.String, userToCreate.Email);
                db.AddInParameter(cmd, "@phone_p", DbType.String, userToCreate.Phone);
                db.AddInParameter(cmd, "@classInSchool_p", DbType.String, userToCreate.ClassInSchool);
                db.AddInParameter(cmd, "@userType_p", DbType.String, userToCreate.UserType);
                db.AddInParameter(cmd, "@comments_p", DbType.String, userToCreate.Comments);//will be null 
                db.AddInParameter(cmd, "@creator_p", DbType.String, creator);
                db.AddInParameter(cmd, "@userIDCounter", DbType.Int32);
                db.AddInParameter(cmd, "@latestUData", DbType.Int32);
                db.AddOutParameter(cmd, "@newuserID", DbType.Int32, ret);

                db.AddInParameter(cmd, "@newudataID", DbType.Int32, ret);

                db.ExecuteNonQuery(cmd);
                object test = db.GetParameterValue(cmd, "@newuserID");
                object test2 = db.GetParameterValue(cmd, "@newudataID");

                ret = int.Parse(db.GetParameterValue(cmd, "@newuserID").ToString());
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //Create equipment in the database
        public bool CreateEquipment(Equipment creating)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Create_Equipment");

                db.AddInParameter(cmd, "@name_p", DbType.String, creating.Name);
                db.AddInParameter(cmd, "@description_p", DbType.String, creating.Description);
                db.AddInParameter(cmd, "@active_p", DbType.Boolean, creating.Active);

                db.ExecuteNonQuery(cmd);

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        //get equipment by id
        public Equipment GetEquipmentByID(int id)
        {
            Equipment ret;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_Equipment_By_ID");

                db.AddInParameter(cmd, "@equipmentID_p", DbType.Int32, id);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret = new Equipment(ds.Tables[0].Rows[0].Field<int>("equipmentID"),
                    CaseFormat(ds.Tables[0].Rows[0].Field<String>("name")),
                    ds.Tables[0].Rows[0].Field<String>("description"),
                    ds.Tables[0].Rows[0].Field<bool>("active"));

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }

            return ret;//return created user or null
        }

        //Edit equipment in the database
        public bool EditEquipment(Equipment editing)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Edit_Equipment");

                db.AddInParameter(cmd, "@equipmentID_p", DbType.Int32, editing.EquipmentID);
                db.AddInParameter(cmd, "@name_p", DbType.String, editing.Name);
                db.AddInParameter(cmd, "@description_p", DbType.String, editing.Description);
                db.AddInParameter(cmd, "@active_p", DbType.Boolean, editing.Active);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Exception " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        //Edit a comment to show or hide using the new value for show
        public bool UpdateComment(int id, bool show)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Update_Comment");

                db.AddInParameter(cmd, "@commentID_p", DbType.Int32, id);
                db.AddInParameter(cmd, "@commentID_p", DbType.Boolean, show);

                db.ExecuteNonQuery(cmd);

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        //Create a visit type in the database
        public bool CreateVisitType(VisitType creating)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Create_Visit_Type");

                db.AddInParameter(cmd, "@title_p", DbType.String, creating.Title);
                db.AddInParameter(cmd, "@description_p", DbType.String, creating.Description);

                db.AddInParameter(cmd, "@active_p", DbType.Boolean, creating.Active);

                db.ExecuteNonQuery(cmd);

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Exception " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        //get visit type by id
        public VisitType GetVisitTypeByID(int id)
        {
            VisitType ret;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_Visit_Type_By_ID");

                db.AddInParameter(cmd, "@visitTypeID_p", DbType.Int32, id);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret = new VisitType();
                ret.VisitTypeID = ds.Tables[0].Rows[0].Field<int>("visitTypeID");
                ret.Title = CaseFormat(ds.Tables[0].Rows[0].Field<String>("title"));
                ret.Description = ds.Tables[0].Rows[0].Field<String>("descrip");


                ret.Active = ds.Tables[0].Rows[0].Field<bool>("active");
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;//return created user or null
        }

        //Edit visit type in the database
        public bool EditVisitType(VisitType editing)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Edit_Visit_Type");

                db.AddInParameter(cmd, "@visitTypeID_p", DbType.Int32, editing.VisitTypeID);
                db.AddInParameter(cmd, "@title_p", DbType.String, editing.Title);
                db.AddInParameter(cmd, "@description_p", DbType.String, editing.Description);

                db.AddInParameter(cmd, "@active_p", DbType.Int32, editing.Active);

                db.ExecuteNonQuery(cmd);

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        //Create a visit type in the database
        public bool CreateCertification(Certification creating)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Create_Certification");

                db.AddInParameter(cmd, "@title_p", DbType.String, creating.Title);
                db.AddInParameter(cmd, "@description_p", DbType.String, creating.Description);
                db.AddInParameter(cmd, "@active_p", DbType.Boolean, creating.Active);

                db.ExecuteNonQuery(cmd);

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        //Create a visit type in the database
        public bool EditCertification(Certification editing)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Edit_Certification");

                db.AddInParameter(cmd, "@certificationID_p", DbType.Int32, editing.CertificationID);
                db.AddInParameter(cmd, "@title_p", DbType.String, editing.Title);
                db.AddInParameter(cmd, "@description_p", DbType.String, editing.Description);
                db.AddInParameter(cmd, "@active_p", DbType.Int32, editing.Active);

                db.ExecuteNonQuery(cmd);

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        //get certification by id
        public Certification GetCertificationByID(int id)
        {
            Certification ret;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_Certification_By_ID");

                db.AddInParameter(cmd, "@certificationID_p", DbType.Int32, id);

                DataSet ds = db.ExecuteDataSet(cmd);
                ret = new Certification();

                ret.CertificationID = ds.Tables[0].Rows[0].Field<int>("certificationID");
                ret.Title = CaseFormat(ds.Tables[0].Rows[0].Field<String>("title"));
                ret.Description = ds.Tables[0].Rows[0].Field<String>("description");
                ret.Active = ds.Tables[0].Rows[0].Field<bool>("active");
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;//return created user or null
        }

        //Create a course in the database
        public bool CreateCourse(Course courseToCreate)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Create_Course");

                db.AddInParameter(cmd, "@title_p", DbType.String, courseToCreate.Title);
                db.AddInParameter(cmd, "@code_p", DbType.String, courseToCreate.courseCode);
                db.AddInParameter(cmd, "@dayofweek_p", DbType.Int32, courseToCreate.DayOfWeek ?? 0);
                db.AddInParameter(cmd, "@timeofday_p", DbType.Int32, courseToCreate.TimeOfDay ?? 0);
                db.AddInParameter(cmd, "@duration_p", DbType.Int32, courseToCreate.Duration);
                db.AddInParameter(cmd, "@checkout_p", DbType.Int32, courseToCreate.CheckOut);
                db.AddInParameter(cmd, "@termID_p", DbType.Int32, courseToCreate.Term.TermID);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        public User GetUserByID(int id)
        {
            User ret = new User();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_User_By_ID");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, id);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.UserID = ds.Tables[0].Rows[0].Field<int>("userID");
                ret.LastName = CaseFormat(ds.Tables[0].Rows[0].Field<String>("lastName"));
                ret.FirstName = CaseFormat(ds.Tables[0].Rows[0].Field<String>("firstName"));
                ret.StudentID = ds.Tables[0].Rows[0].Field<String>("studentID");
                ret.CardScan = ds.Tables[0].Rows[0].Field<String>("cardscan");

                ret.Phone = ds.Tables[0].Rows[0].Field<String>("phone");
                ret.Email = ds.Tables[0].Rows[0].Field<String>("email");
                ret.ClassInSchool = ThePoolDBService.CaseFormat(ds.Tables[0].Rows[0].Field<String>("classInSchool"));
                ret.UserType = ThePoolDBService.CaseFormat(ds.Tables[0].Rows[0].Field<String>("userType"));
                ret.Comments = ds.Tables[0].Rows[0].Field<String>("comments");
                ret.Active = ds.Tables[0].Rows[0].Field<bool>("active");
                ret.Staff = ds.Tables[0].Rows[0].Field<bool>("staffFlag");

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;//return created user or null
        }

        //get user for checkout
        public User GetUserForCheckout(int userID)
        {
            User ret = new User();
            try
            {
                ret = GetCompleteUserByID(userID);

                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_User_For_Checkout");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, userID);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.IsInCourse = ds.Tables[0].Rows[0].Field<int>("userIsIn");
                //ret.AssociatedVisitID = ds.Tables[0].Rows[0].Field<int>("associatedVisit");
                ret.CheckInAt = ds.Tables[0].Rows[0].Field<DateTime>("checkInAt");
                ret.CheckOutFor = ds.Tables[0].Rows[0].Field<int>("checkOutFor");
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;//return created user or null
        }

        //get trends for time period
        public Dictionary<string, DataRowCollection> GetTrends(DateTime start, DateTime end)
        {
            Dictionary<string, DataRowCollection> ret = new Dictionary<string, DataRowCollection>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_Trends");

                db.AddInParameter(cmd, "@startDate_p", DbType.DateTime, start);
                db.AddInParameter(cmd, "@endDate_p", DbType.DateTime, end);

                DataSet ds = db.ExecuteDataSet(cmd);


                ret.Add("General", ds.Tables[0].Rows);
                ret.Add("Course", ds.Tables[1].Rows);
                ret.Add("Type", ds.Tables[2].Rows);
                ret.Add("UserType", ds.Tables[3].Rows);
                ret.Add("Totals", ds.Tables[4].Rows);
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //get data for trends by various time blocks. day, dayOfweek, hour of day

        //get trend by hour of day blocks (max = 24)
        public Dictionary<String, int> GetTrendByDay(DateTime start, DateTime end)
        {
            Dictionary<String, int> ret = new Dictionary<String, int>();//initialize space
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_TrendsBreakdown");

                db.AddInParameter(cmd, "@startDate_p", DbType.DateTime, start);
                db.AddInParameter(cmd, "@endDate_p", DbType.DateTime, end);

                DataSet ds = db.ExecuteDataSet(cmd);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ret.Add(row.Field<DateTime>("Date").ToShortDateString(), row.Field<int>("Visits"));
                }
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //get trend by day
        public Dictionary<int, int> GetTrendByHourOfDay(DateTime start, DateTime end)
        {
            Dictionary<int, int> ret = new Dictionary<int, int>();//initialize space
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_TrendsBreakdown");

                db.AddInParameter(cmd, "@startDate_p", DbType.DateTime, start);
                db.AddInParameter(cmd, "@endDate_p", DbType.DateTime, end);

                DataSet ds = db.ExecuteDataSet(cmd);

                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    ret.Add(row.Field<int>("Hour"), row.Field<int>("Visits"));
                }
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //get trend by day, day specified by integer
        public Dictionary<int, int> GetTrendByDayOfWeek(DateTime start, DateTime end)
        {
            Dictionary<int, int> ret = new Dictionary<int, int>();//initialize space
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_TrendsBreakdown");

                db.AddInParameter(cmd, "@startDate_p", DbType.DateTime, start);
                db.AddInParameter(cmd, "@endDate_p", DbType.DateTime, end);

                DataSet ds = db.ExecuteDataSet(cmd);

                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    ret.Add(row.Field<int>("DayOfWeek"), row.Field<int>("Visits"));
                }
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //get a user's breakdown by activity
        public Dictionary<String, int> GetUserTrends(DateTime start, DateTime end, int userID)
        {
            Dictionary<String, int> ret = new Dictionary<String, int>();//initialize space
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_User_Trends");

                db.AddInParameter(cmd, "@startDate_p", DbType.DateTime, start);
                db.AddInParameter(cmd, "@endDate_p", DbType.DateTime, end);
                db.AddInParameter(cmd, "@userID_p", DbType.Int32, userID);

                DataSet ds = db.ExecuteDataSet(cmd);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ret.Add(row.Field<String>("TITLE"), row.Field<int>("TOTAL"));
                }
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        public List<User> SearchUsersByLastname(String lastname)//returns a list of users with the same lastname
        {
            List<User> ret = new List<User>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_User_By_LastName");

                db.AddInParameter(cmd, "@lastname_p", DbType.String, lastname);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new User(row.Field<int>("userID"),
                                 CaseFormat(row.Field<String>("lastName")), CaseFormat(row.Field<String>("firstName")),
                                 row.Field<String>("userType")));

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;//return empty list if no users with the same lastname are found
        }

        public List<User> SearchBySimilarLastNames(String lastname)//returns a list of users with a similar lastname by first 3 letters
        {
            List<User> ret = new List<User>();
            //similar lastnames
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_User_By_SimilarLastName");
                String stem = (lastname.Length < 4 ? lastname : lastname.Substring(0, 3));

                db.AddInParameter(cmd, "@stem_p", DbType.String, stem);//first three letters

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new User(row.Field<int>("userID"),
                                 CaseFormat(row.Field<String>("lastName")), CaseFormat(row.Field<String>("firstName"))));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }
        //user exists by lastname and firstname
        public bool UserExistsByName(String lastname, String firstname)
        {
            int ret = 0;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("User_Exists_ByName");

                db.AddInParameter(cmd, "@lastname_p", DbType.String, lastname.TrimEnd(new char[] { ' ' }).TrimStart(new char[] { ' ' }));
                db.AddInParameter(cmd, "@firstname_p", DbType.String, firstname.TrimEnd(new char[] { ' ' }).TrimStart(new char[] { ' ' }));

                DataSet ds = db.ExecuteDataSet(cmd);

                ret = ds.Tables[0].Rows[0].Field<int>("exists");//get if user exists
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return (ret != 0);//return if there are users that exist with the same last and firstnames
                              //true if users exist
        }

        public User SearchUsersByCardscan(String cardscan)
        {
            User ret;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_User_By_Cardscan");

                db.AddInParameter(cmd, "@cardscan_p", DbType.String, HashCodeTrue(cardscan));

                DataSet ds = db.ExecuteDataSet(cmd);
                ret = (from row in ds.Tables[0].AsEnumerable()
                       select (new User(row.Field<int>("userID"),
                           row.Field<String>("firstName"),
                           row.Field<String>("lastName")))).FirstOrDefault();

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;//return list of users with this lastname or empty list
        }

        //adds certifications to the user with UserID adds certs with cert id's adding
        public bool AddCertifications(int userID, List<int> adding, int staffID)
        {
            try
            {
                //query loop
                for (int i = 0; i < adding.Count; i++)//for each user to enroll
                {
                    Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                    DbCommand cmd = db.GetStoredProcCommand("Add_Certifications");

                    db.AddInParameter(cmd, "@userID_p", DbType.Int32, userID);
                    db.AddInParameter(cmd, "@certificationID_p", DbType.Int32, adding[i]);
                    db.AddInParameter(cmd, "@staffID_p", DbType.Int32, staffID);

                    db.ExecuteNonQuery(cmd);//add user
                }
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        //clears the enrollment and changes it to the list of userID's associated with enrolled
        public bool AdjustEnrollment(int courseID, List<int> enrolled)
        {
            try
            {
                List<User> currentCourse = GetCourseCompleteByID(courseID).Enrolled;
                IList<int> currentEnrolled = currentCourse.Select(x => x.UserID).ToArray();
                //first remove all currently enrolled that were removed
                for (int i = 0; i < currentEnrolled.Count; i++)//for each user to enroll
                {
                    if (!enrolled.Contains(currentEnrolled[i]))
                    {
                        Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                        DbCommand cmd = db.GetStoredProcCommand("DeEnroll_User");

                        db.AddInParameter(cmd, "@courseID_p", DbType.Int32, courseID);
                        db.AddInParameter(cmd, "@userID_p", DbType.Int32, currentEnrolled[i]);

                        db.ExecuteNonQuery(cmd);//add user
                    }
                }
                //second query loop
                //add new enrollees
                for (int i = 0; i < enrolled.Count; i++)//for each user to enroll
                {
                    if (!currentEnrolled.Contains(enrolled[i]))
                    {
                        Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                        DbCommand cmd = db.GetStoredProcCommand("Enroll_User");

                        db.AddInParameter(cmd, "@courseID_p", DbType.Int32, courseID);
                        db.AddInParameter(cmd, "@userID_p", DbType.Int32, enrolled[i]);

                        db.ExecuteNonQuery(cmd);//add user
                    }
                }
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }


        //get a course for editing without enrollment list
        public Course GetCourseByID(int id)
        {
            Course ret;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_Course_By_ID");

                db.AddInParameter(cmd, "@courseID_p", DbType.Int32, id);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret = new Course(ds.Tables[0].Rows[0].Field<int>("crn"),
                    ds.Tables[0].Rows[0].Field<String>("code"),
                    ThePoolDBService.CaseFormat(CaseFormat(ds.Tables[0].Rows[0].Field<String>("title"))),

                    ds.Tables[0].Rows[0].Field<int?>("dayOfWeek") ?? 0,
                    ds.Tables[0].Rows[0].Field<int?>("duration") ?? 0,
                    ds.Tables[0].Rows[0].Field<int?>("timeOfDay") ?? 0,
                    ds.Tables[0].Rows[0].Field<bool>("checkout"),

                    new Term(ds.Tables[0].Rows[0].Field<int>("termID"), ds.Tables[0].Rows[0].Field<String>("quarter"),
                        ds.Tables[0].Rows[0].Field<String>("year"),
                        ds.Tables[0].Rows[0].Field<DateTime>("startt"),
                        ds.Tables[0].Rows[0].Field<DateTime>("endd")));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;//return created user or null
        }

        //get a course for editing WITH COMPLETE enrollment list
        public Course GetCourseCompleteByID(int id)
        {
            Course ret;
            try
            {
                ret = this.GetCourseByID(id);//get general course information
                if (ret != null)
                {
                    Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                    DbCommand cmd = db.GetStoredProcCommand("List_Enrolled");

                    db.AddInParameter(cmd, "@courseID_p", DbType.Int32, id);

                    DataSet ds = db.ExecuteDataSet(cmd);

                    ret.Enrolled.AddRange(from row in ds.Tables[0].AsEnumerable()
                                          select new User(row.Field<int>("userID"),
                                             CaseFormat(row.Field<String>("firstName")), CaseFormat(row.Field<String>("lastName"))));
                }
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;//return created user or null
        }

        //get a user from the database WITH certification list and enrolled classes
        public User GetCompleteUserByID(int id)
        {
            User ret;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_Complete_User_By_ID");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, id);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret = new User();

                ret.UserID = ds.Tables[0].Rows[0].Field<int>("userID");
                ret.LastName = CaseFormat(ds.Tables[0].Rows[0].Field<String>("lastName"));
                ret.FirstName = CaseFormat(ds.Tables[0].Rows[0].Field<String>("firstName"));
                ret.StudentID = ds.Tables[0].Rows[0].Field<String>("studentID");
                ret.CardScan = ds.Tables[0].Rows[0].Field<String>("cardscan");

                ret.Phone = ds.Tables[0].Rows[0].Field<String>("phone");
                ret.Email = ds.Tables[0].Rows[0].Field<String>("email");
                ret.ClassInSchool = ThePoolDBService.CaseFormat(ds.Tables[0].Rows[0].Field<String>("classInSchool"));
                ret.UserType = ThePoolDBService.CaseFormat(ds.Tables[0].Rows[0].Field<String>("userType"));
                ret.Comments = ds.Tables[0].Rows[0].Field<String>("comments");
                ret.Active = ds.Tables[0].Rows[0].Field<bool>("active");
                ret.Staff = ds.Tables[0].Rows[0].Field<bool>("staffFlag");

                ret.Classes.AddRange(from row in ds.Tables[1].AsEnumerable()
                                     select new Course(row.Field<int>("CRN"),
                                         row.Field<String>("title"),
                                         row.Field<bool>("checkout")));

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;//return created user or null
        }

        #region UserSummary

        //get a list of all the users history as user objects containing all user edits
        public List<User> GetUserHistoryByID(int id)
        {
            List<User> ret = new List<User>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_UserHistory_By_ID");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, id);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new User(row.Field<int>("userID"),
                                 CaseFormat(row.Field<String>("firstName")),
                                 CaseFormat(row.Field<String>("lastName")),
                                 row.Field<String>("studentID"),
                                 row.Field<String>("cardscan"),

                                 row.Field<String>("phone"),
                                 row.Field<String>("email"),
                                 CaseFormat(row.Field<String>("classInSchool")),
                                 CaseFormat(row.Field<String>("userType")),
                                    true,
                                 row.Field<bool>("staffFlag"),
                                 row.Field<String>("comments"),
                                 row.Field<DateTime>("CreatedDate")
                                 ));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;//return list of user history
        }

        //get a list of all the users courses ever enrolled in as user objects containing all user edits
        public List<Course> GetUserCourseHistoryByID(int id)
        {
            List<Course> ret = new List<Course>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_UserCourseHistory_By_ID");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, id);

                DataSet ds = db.ExecuteDataSet(cmd);

                //ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                //             select new Course(row.Field<int>("courseID"),
                //                               CaseFormat(row.Field<String>("title")),
                //                               row.Field<String>("code"),
                //                               row.Field<int?>("dayOfWeek") ?? 0,
                //                               row.Field<int?>("duration") ?? 0,
                //                               row.Field<int?>("timeOfDay") ?? 0,
                //                               row.Field<bool>("checkout")));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                                                 excp.Message, excp);
                throw (myExcp);
            }
            return ret; //return list of user history
        }

        //get a list of all the users courses ever enrolled in as user objects containing all user edits
        public List<CertificationHist> GetUserCertHistoryByID(int id)
        {
            List<CertificationHist> ret = new List<CertificationHist>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_UserCertHistory_By_ID");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, id);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new CertificationHist(row.Field<int>("certificationID"),
                                 CaseFormat(row.Field<String>("title")),
                                 row.Field<DateTime>("Posted"),
                                 row.Field<DateTime>("Removed")));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                                                 excp.Message, excp);
                throw (myExcp);
            }
            return ret; //return list of user history
        }

        //get a list of all the users visits
        public List<Visit> GetUserVisitsHistoryByID(int id)
        {
            List<Visit> ret = new List<Visit>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_UserVisitsHistory_By_ID");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, id);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Visit(
                                 row.Field<int>("visitID"),
                                 row.Field<string>("visitType"),
                                 row.Field<string>("code"),
                                 row.Field<DateTime>("dateTime"),
                                 row.Field<int>("inOut")
                                 ));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Database exception: " +
                                                 excp.Message, excp);
                throw (myExcp);
            }
            return ret; //return list of user history
        }

        #endregion

        //submit a course visit to the database        
        public int SubmitCourseVisit(int userID, int courseID, bool checkout, string staffUser, string ip)
        {
            int ret = -1;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Submit_Course_Visit");

                db.AddInParameter(cmd, "@courseID_p", DbType.Int32, courseID);
                db.AddInParameter(cmd, "@userID_p", DbType.Int32, userID);
                db.AddInParameter(cmd, "@checkin_p", DbType.Boolean, checkout);
                db.AddInParameter(cmd, "@sso_p", DbType.String, staffUser);
                db.AddInParameter(cmd, "@IP_p", DbType.String, ip);
                DataSet ds = db.ExecuteDataSet(cmd);

                ret = ds.Tables[0].Rows[0].Field<int>("visitID");//get back this visit ID
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //checkout from the course the user is currently enrolled in
        public int SubmitCheckout(int userID, bool valid, string staffUser, string ip, int associatedVisit)
        {
            int ret = -1;
            int checkoutVal = -1;
            if (valid)
            { checkoutVal = 0; }//set checkout value
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Submit_Course_Checkout");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, userID);
                db.AddInParameter(cmd, "@valid_p", DbType.Int32, checkoutVal);//if valid checkout, send 0 else sent -1
                db.AddInParameter(cmd, "@sso_p", DbType.String, staffUser);
                db.AddInParameter(cmd, "@IP_p", DbType.String, ip);
                db.AddInParameter(cmd, "@associatedVisit_p", DbType.Int32, associatedVisit);

                DataSet ds = db.ExecuteDataSet(cmd);

                //ret = ds.Tables[0].Rows[0].Field<int>("visitID");//get back this visit ID
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //submit equipment usage to the database
        public bool SubmitEquipment(int[] equipment, int visitID)
        {
            try
            {

                for (int i = 0; i < equipment.Length; i++)
                {
                    Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                    DbCommand cmd = db.GetStoredProcCommand("Submit_Equipment");

                    db.AddInParameter(cmd, "@visitID_p", DbType.Int32, visitID);
                    db.AddInParameter(cmd, "@equipmentID_p", DbType.Int32, equipment[i]);

                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        //submit a general visit
        public int SubmitGeneralVisit(int userID, int visitTypeID, string staffUser, string ip)
        {
            int ret = -1;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Submit_General_Visit");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, userID);
                db.AddInParameter(cmd, "@visitTypeID_p", DbType.Int32, visitTypeID);
                db.AddInParameter(cmd, "@sso_p", DbType.String, staffUser);
                db.AddInParameter(cmd, "@IP_p", DbType.String, ip);
                //force change
                DataSet ds = db.ExecuteDataSet(cmd);

                ret = ds.Tables[0].Rows[0].Field<int>("visitID");//get back this visit ID
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        #region IP_Visit_Report

        //list avaliable visit types for a user with this ID
        public List<Visit> ListVisitsForReport(DateTime start, DateTime end)
        {
            List<Visit> ret = new List<Visit>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Generate_VisitList_Complete");

                db.AddInParameter(cmd, "@start_p", DbType.DateTime, start);
                db.AddInParameter(cmd, "@end_p", DbType.DateTime, end);

                DataSet ds = db.ExecuteDataSet(cmd);
                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Visit(
                                 row.Field<Int32>("VisitID"),
                                 row.Field<Int32>("UserID"),
                                 new User(row.Field<int>("UserID"), ThePoolDBService.CaseFormat(row.Field<String>("firstName"))
                                     , ThePoolDBService.CaseFormat(row.Field<String>("lastName"))),
                                 row.Field<DateTime>("dateTime"),
                                 row.Field<String>("Type"),
                                 row.Field<Int32>("inOut"),
                                 row.Field<String>("IP"),
                                 row.Field<String>("SSO"),
                                 row.Field<bool>("lineValid"),
                                 row.Field<int?>("associatedVisit")
                                 ));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //change this visitID's lineValid to valid flag
        public bool UpdateVisitValidation(int visitID, bool valid)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Update_Visit_Valid");

                db.AddInParameter(cmd, "@visitID_p", DbType.Int32, visitID);
                db.AddInParameter(cmd, "@valid_p", DbType.Boolean, valid);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        #endregion

        //list avaliable visit types for a user with this ID
        public List<VisitType> ListAvailableVisitTypes(int id)
        {
            List<VisitType> ret = new List<VisitType>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Lists_Avaliable_Visit_Types");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, id);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new VisitType(row.Field<int>("visitTypeID"),

                                 CaseFormat(row.Field<String>("title")),
                                 row.Field<bool>("active")));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            //close the connection

            return ret;
        }

        //returns a list of certifications that this user does NOT have
        public List<Certification> ListAvailableCerts(int userID)
        {
            List<Certification> ret = new List<Certification>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_Avaliable_Certifications");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, userID);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Certification(row.Field<int>("certificationID"),
                                 CaseFormat(CaseFormat(row.Field<String>("title"))),
                                 row.Field<bool>("active")));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }

            return ret;
        }

        //returns a list of users that are enrolled in this course
        public List<User> GetNotEnrolled(int courseID)
        {
            List<User> ret = new List<User>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_Not_Enrolled");

                db.AddInParameter(cmd, "@courseID_p", DbType.Int32, courseID);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new User(row.Field<int>("userID"),
                                 CaseFormat(row.Field<String>("firstName")),
                                 CaseFormat(row.Field<String>("lastName"))));

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }

            return ret;
        }


        //edit an existing course in the database
        public bool EditCourse(Course courseToEdit)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Edit_Course");


                db.AddInParameter(cmd, "@courseID_p", DbType.Int32, courseToEdit.CRN);

                db.AddInParameter(cmd, "@title_p", DbType.String, courseToEdit.Title);
                db.AddInParameter(cmd, "@code_p", DbType.String, courseToEdit.courseCode);
                db.AddInParameter(cmd, "@dayofweek_p", DbType.Int32, courseToEdit.DayOfWeek ?? 0);
                db.AddInParameter(cmd, "@timeofday_p", DbType.Int32, courseToEdit.TimeOfDay ?? 0);
                db.AddInParameter(cmd, "@duration_p", DbType.Int32, courseToEdit.Duration);
                db.AddInParameter(cmd, "@checkout_p", DbType.Int32, courseToEdit.CheckOut);
                db.AddInParameter(cmd, "@termID_p", DbType.Int32, courseToEdit.Term.TermID);

                db.ExecuteNonQuery(cmd);

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        //edit user
        public bool EditUser(User user, String creator)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Edit_User");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, user.UserID);

                db.AddInParameter(cmd, "@firstName_p", DbType.String, user.FirstName.TrimEnd(new char[] { ' ' }).TrimStart(new char[] { ' ' }));
                db.AddInParameter(cmd, "@lastName_p", DbType.String, user.LastName.TrimEnd(new char[] { ' ' }).TrimStart(new char[] { ' ' }));
                db.AddInParameter(cmd, "@sid_p", DbType.String, ThePoolDBService.HashCode(user.StudentID));
                String encoded = ThePoolDBService.HashCode(user.CardScan);//encode card scan
                db.AddInParameter(cmd, "@cardscan_p", DbType.String, encoded);

                db.AddInParameter(cmd, "@email_p", DbType.String, user.Email);
                db.AddInParameter(cmd, "@phone_p", DbType.String, user.Phone);
                db.AddInParameter(cmd, "@classInSchool_p", DbType.String, user.ClassInSchool);
                db.AddInParameter(cmd, "@userType_p", DbType.String, user.UserType);
                db.AddInParameter(cmd, "@comments_p", DbType.String, user.Comments);
                db.AddInParameter(cmd, "@staffFlag_p", DbType.Boolean, user.Staff);

                db.AddInParameter(cmd, "@creator_p", DbType.String, creator);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        //list all active comments
        public List<Comment> ListComments()
        {
            List<Comment> ret = new List<Comment>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_Comments");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Comment(row.Field<int>("commentID"),
                                 row.Field<String>("title"),
                                 row.Field<DateTime>("dateTimePosted"),
                                 row.Field<String>("comment")));
            }
            catch (Exception excp)
            {
                //Exception myExcp = new Exception("Could not add user. Error: " +
                //excp.Message, excp);

                Console.WriteLine(excp);
                //throw (excp);
            }
            return ret;
        }

        //list all comments both active and inactive!
        public List<Comment> ListAllComments()
        {
            List<Comment> ret = new List<Comment>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_All_Comments");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Comment(row.Field<int>("commentID"),
                                 row.Field<String>("title"),
                                 row.Field<DateTime>("dateTimePosted"),
                                 row.Field<String>("comment")));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //list all courses (both active and inactive)
        public List<Course> ListAllCourses()
        {
            List<Course> ret = new List<Course>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_All_Courses");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Course(row.Field<int>("crn"),
                                 row.Field<String>("code"),
                                 CaseFormat(row.Field<String>("title")),
                                 row.Field<int?>("dayOfWeek") ?? 0,
                                 row.Field<int?>("duration") ?? 0,
                                 row.Field<int?>("timeOfDay") ?? 0,
                                 row.Field<bool>("checkout"),
                                     new Term(row.Field<int>("termID"), row.Field<String>("quarter"),
                        row.Field<String>("year"),
                        row.Field<DateTime>("startt"),
                        row.Field<DateTime>("endd"))));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }

            return ret;
        }

        //generate a master report
        public Report GenerateReport(Report creating)
        {
            //creating.Data.Add(new ReportRow(1, "tester", "bob", "studentID", "STUD", "FROSH", false, DateTime.Now,
            //    "swimming", null, true));
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Generate_Master_Report");

                db.AddInParameter(cmd, "@startDate_p", DbType.DateTime, creating.Start);
                db.AddInParameter(cmd, "@endDate_p", DbType.DateTime, creating.Finish.AddDays(1));


                DataSet ds = db.ExecuteDataSet(cmd);


                creating.Data.AddRange(from row in ds.Tables[0].AsEnumerable()
                                       select new ReportRow(row.Field<int>("userID"),
                                           row.Field<String>("lastName"),

                                           row.Field<String>("firstName"),

                                           row.Field<String>("studentID"),
                                           row.Field<String>("userType"),
                                           row.Field<String>("classInSchool"),

                                           row.Field<bool>("staffFlag"),

                                           row.Field<DateTime>("dateTimee"),


                                           row.Field<int>("courseID")));


            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }

            return creating;
        }



        public UTReport GenerateUTReport(UTReport creating)
        {
            List<UTReportRow> ret;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Generate_User_Total_Report");

                db.AddInParameter(cmd, "@start_p", DbType.DateTime, creating.Start);
                db.AddInParameter(cmd, "@end_p", DbType.DateTime, creating.Finish);

                DataSet ds = db.ExecuteDataSet(cmd);

                creating.Data.AddRange(from row in ds.Tables[0].AsEnumerable()
                                       select new UTReportRow(row.Field<int>("totalVisits"),
                                           new User(row.Field<int>("userID"),
                                               CaseFormat(row.Field<String>("firstName")),
                                               CaseFormat(row.Field<String>("lastName")),
                                               row.Field<String>("userType"))));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }

            return creating;
        }

        //generate a master report
        public List<Visit> GenerateCourseReportVisits(int courseID)
        {
            List<Visit> ret = new List<Visit>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Generate_Course_Report");

                db.AddInParameter(cmd, "@courseID_p", DbType.Int32, courseID);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Visit(row.Field<int>("visitID"),
                                 row.Field<DateTime>("dateTime"),
                                 row.Field<int>("inOut"),
                                 new User(row.Field<int>("userID"),
                                     CaseFormat(row.Field<String>("firstName")),
                                     CaseFormat(row.Field<String>("lastName")))));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }

            return ret;
        }

        //get all avaliable types of users
        public List<String> GetUserTypes()
        {
            List<String> ret = new List<String>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_User_Types");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select CaseFormat(row.Field<String>("title")));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }

            return ret;
        }

        //returns a complete list of all users
        public List<User> ListUsers()
        {
            List<User> ret = new List<User>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_Users");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new User(row.Field<int>("userID"),
                                 CaseFormat(row.Field<String>("firstName")),
                                 CaseFormat(row.Field<String>("lastName"))));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }

            return ret;
        }

        //returns a complete list of all users currently checked in for a course that requires checkout
        public List<User> ListUsersIn()
        {
            List<User> ret = new List<User>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_Users_In");

                DataSet ds = db.ExecuteDataSet(cmd);


                ret.AddRange(from row in ds.Tables[0].AsEnumerable()

                             select new User(row.Field<int>("userID"),

                                 CaseFormat(row.Field<String>("firstName")),
                                 CaseFormat(row.Field<String>("lastName"))));
                //row.Field<int>("userIsIn")));
                //row.Field<int>("associatedVisit")));

                //figure this out

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }

            return ret;
        }

        //returns  a list of all members that have staff status
        public List<User> ListStaff()
        {
            List<User> ret = new List<User>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_Staff");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new User(row.Field<int>("userID"),
                                 CaseFormat(row.Field<String>("firstName")),
                                 CaseFormat(row.Field<String>("lastName"))));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //returns a complete list of all users (both active and inactive!)
        public List<User> ListAllUsers()
        {
            List<User> ret = new List<User>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_All_Users");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new User(row.Field<int>("userID"),
                                 CaseFormat(row.Field<String>("firstName")),
                                 CaseFormat(row.Field<String>("lastName"))));

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //returns a complete list of all users active and inactive and all data for bulk user action searching
        public List<User> ListAllUsersComplete()
        {
            List<User> ret = new List<User>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_All_Users");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new User(row.Field<int>("userID"),
                                 CaseFormat(row.Field<String>("firstName")),
                                 CaseFormat(row.Field<String>("lastName")),
                                 row.Field<String>("studentID"),
                                 row.Field<String>("cardscan"),

                                 row.Field<String>("phone"),
                                 row.Field<String>("email"),
                                 row.Field<String>("classInSchool"),
                                 row.Field<String>("userType"),
                                 row.Field<bool>("active"),
                                 row.Field<bool>("staffFlag"),
                                 row.Field<String>("comments")));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //returns a complete list of users with searching and filtering
        public List<User> ListUsersFiltered(String filter, String search)
        {
            List<User> ret = new List<User>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Search_Filter_Users");

                db.AddInParameter(cmd, "@filter_p", DbType.String, filter.ToUpper());//uppercase filter and search term
                db.AddInParameter(cmd, "@search_p", DbType.String, search.ToUpper());

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new User(row.Field<int>("userID"),
                                 CaseFormat(row.Field<String>("firstName")),
                                 CaseFormat(row.Field<String>("lastName"))));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //list all active equipment
        public List<Equipment> ListEquipment()
        {
            List<Equipment> ret = new List<Equipment>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_Equipment");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Equipment(row.Field<int>("equipmentID"),
                                 CaseFormat(row.Field<String>("name")),
                                 row.Field<String>("description"),
                                 row.Field<bool>("active")));

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //list all equipment (both active and inactive!)
        public List<Equipment> ListAllEquipment()
        {
            List<Equipment> ret = new List<Equipment>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_All_Equipment");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Equipment(row.Field<int>("equipmentID"),
                                 CaseFormat(row.Field<String>("name")),
                                 row.Field<String>("description"),
                                 row.Field<bool>("active")));

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //list visit types
        public List<VisitType> ListVisitTypes()
        {
            List<VisitType> ret = new List<VisitType>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_Visit_Types");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new VisitType(row.Field<int>("visitTypeID"),

                                 CaseFormat(row.Field<String>("title")),
                                 row.Field<bool>("active")));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //list all visit types (active and inactive!)
        public List<VisitType> ListAllVisitTypes()
        {
            List<VisitType> ret = new List<VisitType>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_All_Visit_Types");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new VisitType(row.Field<int>("visitTypeID"),

                                 CaseFormat(row.Field<String>("title")),
                                 row.Field<String>("descrip"),
                                 row.Field<bool>("active")));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //list certifications
        public List<Certification> ListCertifications()
        {
            List<Certification> ret = new List<Certification>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_Certifications");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Certification(row.Field<int>("certificationID"),
                                 CaseFormat(row.Field<String>("title")),
                                 row.Field<bool>("active")));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        //list all certifications (both active and inactive!)
        public List<Certification> ListAllCertifications()
        {
            List<Certification> ret = new List<Certification>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_All_Certifications");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Certification(row.Field<int>("certificationID"),
                                 CaseFormat(row.Field<String>("title")),
                                 row.Field<String>("description"),
                                 row.Field<bool>("active")));
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }
        //updates the activation status of a user
        public void UpdateActivation(int[] userIDs, bool status)
        {
            try
            {
                for (int i = 0; i < userIDs.Length; i++)
                {
                    Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                    DbCommand cmd = db.GetStoredProcCommand("Update_User_Activation");

                    db.AddInParameter(cmd, "@userID_p", DbType.Int32, userIDs[i]);
                    db.AddInParameter(cmd, "@active_p", DbType.Int32, status);
                    db.ExecuteNonQuery(cmd);
                }

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return;
        }
        //updates the activation status of a user
        public void UpdateStaff(int[] userIDs, bool status)
        {
            try
            {
                for (int i = 0; i < userIDs.Length; i++)
                {
                    Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                    DbCommand cmd = db.GetStoredProcCommand("Update_User_Staff");

                    db.AddInParameter(cmd, "@userID_p", DbType.Int32, userIDs[i]);
                    db.AddInParameter(cmd, "@active_p", DbType.Int32, status);
                    db.ExecuteNonQuery(cmd);
                }

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return;
        }
        //updates the user type of a user
        public void UpdateUserType(int[] userIDs, String type, int staff)
        {
            try
            {
                User temp = this.GetUserByID(staff);
                String creator = temp.FirstName + " " + temp.LastName;
                for (int i = 0; i < userIDs.Length; i++)
                {
                    Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                    DbCommand cmd = db.GetStoredProcCommand("Update_User_Type");

                    db.AddInParameter(cmd, "@userID_p", DbType.Int32, userIDs[i]);
                    db.AddInParameter(cmd, "@userType_p", DbType.String, type);
                    db.AddInParameter(cmd, "@creator_p", DbType.String, creator);
                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return;
        }
        //updates the class in school of a user
        public void UpdateUserClass(int[] userIDs, String classInSchool, int staff)
        {
            try
            {
                User temp = this.GetUserByID(staff);
                String creator = temp.FirstName + " " + temp.LastName;
                for (int i = 0; i < userIDs.Length; i++)
                {
                    Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                    DbCommand cmd = db.GetStoredProcCommand("Update_User_Class");

                    db.AddInParameter(cmd, "@userID_p", DbType.Int32, userIDs[i]);
                    db.AddInParameter(cmd, "@userClass_p", DbType.String, classInSchool);
                    db.AddInParameter(cmd, "@creator_p", DbType.String, creator);
                    db.ExecuteNonQuery(cmd);
                }

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return;
        }
        //update bulk certifications
        public void UpdateCertifications(int[] userIDs, int certID, bool add, int staff)
        {
            try
            {
                for (int i = 0; i < userIDs.Length; i++)
                {
                    if (add)//if add is true then
                    {
                        Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                        DbCommand cmd = db.GetStoredProcCommand("Add_Certifications");

                        db.AddInParameter(cmd, "@userID_p", DbType.Int32, userIDs[i]);
                        db.AddInParameter(cmd, "@certificationID_p", DbType.String, certID);
                        db.AddInParameter(cmd, "@staffID_p", DbType.String, staff);
                        db.ExecuteNonQuery(cmd);
                    }
                    else
                    {
                        Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                        DbCommand cmd = db.GetStoredProcCommand("Remove_Certification");

                        db.AddInParameter(cmd, "@userID_p", DbType.Int32, userIDs[i]);
                        db.AddInParameter(cmd, "@certificationID_p", DbType.String, certID);
                        db.ExecuteNonQuery(cmd);
                    }
                }
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return;
        }

        public List<Visit> ListUserVisitsForCheckout(int userID, int courseID)
        {
            List<Visit> ret = new List<Visit>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_User_Visits_ForCheckout");

                db.AddInParameter(cmd, "@userID_p", DbType.Int32, userID);
                db.AddInParameter(cmd, "@courseID_p", DbType.Int32, courseID);

                DataSet ds = db.ExecuteDataSet(cmd);
                //public SwimTime(int id, String title, String Term, String Course, double offset,
                //int type, double minPerVisit, double maxPerVisit, DateTime start, DateTime end, double rep)
                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Visit(row.Field<int>("visitID"),
                                 row.Field<DateTime>("dateTimee"),
                                 row.Field<TimeSpan>("checkInAt"),
                                 row.Field<TimeSpan>("checkOut")));


            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        public List<Term> ListAllTerms()
        {
            List<Term> ret = new List<Term>();
            try
            {

                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_All_Terms");

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new Term(row.Field<int>("TermID"),
                                 CaseFormat(row.Field<String>("Quarter")),
                                 CaseFormat(row.Field<String>("Year")),
                                 row.Field<DateTime>("startt"),
                                 row.Field<DateTime>("endd"),
                                 row.Field<String>("Title")));
            }
            catch (Exception excp)
            {
                System.Diagnostics.Debug.WriteLine(excp);
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        public bool CreateTerm(Term creating)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Create_Term");

                db.AddInParameter(cmd, "@quarter_p", DbType.String, creating.Quarter);
                db.AddInParameter(cmd, "@year_p", DbType.String, creating.Year);
                db.AddInParameter(cmd, "@start_p", DbType.DateTime, creating.Start);
                db.AddInParameter(cmd, "@end_p", DbType.DateTime, creating.Endd);
                db.AddInParameter(cmd, "@title_p", DbType.String, creating.Title);

                db.ExecuteNonQuery(cmd);

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        public Term GetTermByID(int termID)
        {
            Term ret;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_Term_By_ID");

                db.AddInParameter(cmd, "@termID_p", DbType.Int32, termID);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret = new Term(ds.Tables[0].Rows[0].Field<int>("termID"),
                                 CaseFormat(ds.Tables[0].Rows[0].Field<String>("quarter")),
                                 CaseFormat(ds.Tables[0].Rows[0].Field<String>("year")),
                                 ds.Tables[0].Rows[0].Field<DateTime>("startt"),
                                 ds.Tables[0].Rows[0].Field<DateTime>("endd"),
                                 ds.Tables[0].Rows[0].Field<String>("title"));

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        public bool EditTerm(Term creating)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Edit_Term");

                db.AddInParameter(cmd, "@termID_p", DbType.Int32, creating.TermID);
                db.AddInParameter(cmd, "@quarter_p", DbType.String, creating.Quarter);
                db.AddInParameter(cmd, "@year_p", DbType.String, creating.Year);
                db.AddInParameter(cmd, "@start_p", DbType.DateTime, creating.Start);
                db.AddInParameter(cmd, "@end_p", DbType.DateTime, creating.Endd);
                db.AddInParameter(cmd, "@title_p", DbType.String, creating.Title);

                db.ExecuteNonQuery(cmd);

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        public List<SwimTime> GetRulesByCourse(int courseID)
        {
            List<SwimTime> ret = new List<SwimTime>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_Rules_By_Course");

                db.AddInParameter(cmd, "@courseID_p", DbType.Int32, courseID);

                DataSet ds = db.ExecuteDataSet(cmd);

                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new SwimTime(row.Field<int>("ruleID"),
                                 CaseFormat(row.Field<String>("title")),
                                 new Course(row.Field<int>("courseID"), null),
                                 row.Field<int>("Offset"),
                                 row.Field<int>("ruleType"),
                                 row.Field<int>("minHoursPerVisit"),

                                 row.Field<int>("maxHoursPerVisit"),
                                 row.Field<DateTime>("startDate"),
                                 row.Field<DateTime>("endDate")));

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        public List<SwimTime> ListAllSwimTimeRules()
        {
            List<SwimTime> ret = new List<SwimTime>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("List_All_SwimTimeRules");

                DataSet ds = db.ExecuteDataSet(cmd);
                //public SwimTime(int id, String title, String Term, String Course, double offset,
                //int type, double minPerVisit, double maxPerVisit, DateTime start, DateTime end, double rep)
                ret.AddRange(from row in ds.Tables[0].AsEnumerable()
                             select new SwimTime(row.Field<int>("ruleID"),
                                 CaseFormat(row.Field<String>("title")),
                                 new Course(row.Field<String>("code")),
                                 row.Field<int>("Offset"),
                                 row.Field<int>("ruleType"),
                                 row.Field<int>("minHoursPerVisit"),
                                 row.Field<int>("maxHoursPerVisit"),
                                 row.Field<DateTime>("startDate"),
                                 row.Field<DateTime>("endDate")));

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        public bool CreateSwimTimeRule(SwimTime creating)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Create_SwimTime");

                db.AddInParameter(cmd, "@courseID_p", DbType.Int32, creating.Course.courseCode.ToString());
                db.AddInParameter(cmd, "@minHoursPerVisit_p", DbType.Double, creating.MinHoursPerVisit);
                db.AddInParameter(cmd, "@maxHoursPerVisit_p", DbType.Double, creating.MaxHoursPerVisit);
                db.AddInParameter(cmd, "@ruleType_p", DbType.Int32, creating.Type);
                db.AddInParameter(cmd, "@startDate_p", DbType.DateTime, creating.Start);
                db.AddInParameter(cmd, "@endDate_p", DbType.DateTime, creating.End);



                db.ExecuteNonQuery(cmd);

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        public bool EditSwimTimeRule(SwimTime editing)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Edit_Rule");

                db.AddInParameter(cmd, "@ruleID_p", DbType.Int32, editing.SwimTimeID);
                db.AddInParameter(cmd, "@courseID_p", DbType.Int32, editing.Course.CRN);
                db.AddInParameter(cmd, "@minHoursPerVisit_p", DbType.Double, editing.MinHoursPerVisit);
                db.AddInParameter(cmd, "@maxHoursPerVisit_p", DbType.Double, editing.MaxHoursPerVisit);
                db.AddInParameter(cmd, "@ruleType_p", DbType.Int32, editing.Type);
                db.AddInParameter(cmd, "@startDate_p", DbType.DateTime, editing.Start);
                db.AddInParameter(cmd, "@endDate_p", DbType.DateTime, editing.End);



                db.ExecuteNonQuery(cmd);

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return true;
        }

        public SwimTime GetSwimTimeByID(int swimTimeID)
        {
            SwimTime ret;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThePoolDB");
                DbCommand cmd = db.GetStoredProcCommand("Get_Rule_By_ID");

                db.AddInParameter(cmd, "@ruleID_p", DbType.Int32, swimTimeID);

                DataSet ds = db.ExecuteDataSet(cmd);
                //public SwimTime(int id, String title, String Term, String Course, double offset,
                //int type, double minPerVisit, double maxPerVisit, DateTime start, DateTime end, double rep)
                ret = new SwimTime(ds.Tables[0].Rows[0].Field<int>("ruleID"),
                                 CaseFormat(ds.Tables[0].Rows[0].Field<String>("title")),
                                 new Course(ds.Tables[0].Rows[0].Field<int>("courseID"), null),
                                 ds.Tables[0].Rows[0].Field<int>("Offset"),
                                 ds.Tables[0].Rows[0].Field<int>("ruleType"),
                                 ds.Tables[0].Rows[0].Field<int>("minHoursPerVisit"),
                                 ds.Tables[0].Rows[0].Field<int>("maxHoursPerVisit"),
                                 ds.Tables[0].Rows[0].Field<DateTime>("startDate"),
                                 ds.Tables[0].Rows[0].Field<DateTime>("endDate"));

            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not add user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return ret;
        }

        /*
                //STATIC HELPER METHODS
                //send a message to the specified email account
                private static bool EmailMessage(String subject, String message, String creator)
                {
                    MailAddress from = new MailAddress(ConfigurationManager.AppSettings["SenderEmailNoReply"]);
                    MailAddress to = new MailAddress(ConfigurationManager.AppSettings["MailTo"]);
                    String body = message + "\n -- " + creator;
                    String subjectStr = "Rockwall: " + creator;
                    MailMessage mail = new MailMessage(from, to);
                    mail.Body = body;
                    mail.Subject = subjectStr;

                    SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["EmailSMTP"]);
                    smtp.Send(mail);
                    return true;
                }
        */
        private static String CaseFormat(String input)
        {
            if (!(String.IsNullOrWhiteSpace(input)))
            {
                return input[0] + input.Substring(1, input.Length - 1).ToLower();
            }
            return input;
        }


        //Encryption SHA1
        private static string HashCode(string str)
        {
            string rethash = "";
            try
            {
                if (String.IsNullOrEmpty(str) || str.Length > 19)
                {
                    return str;//return string if string is empty or already hashed (long length)
                }
                System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1.Create();
                System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
                byte[] combined = encoder.GetBytes(str);
                hash.ComputeHash(combined);
                rethash = Convert.ToBase64String(hash.Hash);
            }
            catch (Exception ex)
            {
                string strerr = "Error in HashCode : " + ex.Message;
            }
            return rethash;
        }

        //Encryption SHA1
        private static string HashCodeTrue(string str)//get hash code regardless of already hashed
        {
            string rethash = "";
            try
            {
                System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1.Create();
                System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
                byte[] combined = encoder.GetBytes(str);
                hash.ComputeHash(combined);
                rethash = Convert.ToBase64String(hash.Hash);
            }
            catch (Exception ex)
            {
                string strerr = "Error in HashCode : " + ex.Message;
            }
            return rethash;
        }
    }
}

      
