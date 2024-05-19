using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COMMON
{
    public class Enums
    {
        public enum UserRole
        {

            SuperAdmin = 1,
            Admin = 2
        }
        public enum StatusInfo
        {
            Open = 1,
            Assign_To_Help_Desk = 2,
            Work_In_Progress = 3,
            Hold = 4,
            Breach = 5,
            Closed = 6
        }
        public enum OperationType
        {

            PRIMARY_COLLECTION = 1,
            SECONDARY_COLLECTION = 2,
            TERTIARY_COLLECTION = 3
        }
        public enum PointCategory
        {
            START_POINT=5,
            END_POINT=6
        }
    }
}
