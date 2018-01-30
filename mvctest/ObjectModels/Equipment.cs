using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class Equipment
    {
        public int EquipmentID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public bool Active { get; set; }


        //constructors
        public Equipment()
        {

        }

        public Equipment(int equipmentID, String name, bool active)
        {
            EquipmentID = equipmentID;
            Name = name;
            Active = active;
        }
        public Equipment(int equipmentID, String name, String desc, bool active)
        {
            EquipmentID = equipmentID;
            Description = desc;
            Name = name;
            Active = active;
        }
    }
}