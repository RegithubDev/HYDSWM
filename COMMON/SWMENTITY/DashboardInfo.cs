using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COMMON.SWMENTITY
{
    public class NDayCollectionInfo
    {
        [Key]
        public int ID { get; set; }
        public int COLLECTED { get; set; }
        public int TOTALHOUSE { get; set; }
        public string TDATE { get; set; }
    }
    public class DashboardNotification
    {
        [Key]
        public string UserId { get; set; }
        public int TotalHouseHold { get; set; }
        public int CollectedHouse { get; set; }
        public int NotCollectedHouse { get; set; }
        public int TOTALEMPLOYEE { get; set; }
        public int TOTALCSI { get; set; }
        public int TOTALSUPERVISOR { get; set; }
        public int TOTALDRIVER { get; set; }
        public int TOTALSTAFF { get; set; }

        public int PRESENTEMP { get; set; }
        public int ABSENTEMP { get; set; }

    }
    public class MapViewInfo
    {
        [Key]
        public int HouseId { get; set; }
        public string UHouseId { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string WardNo { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string CircleName { get; set; }
        public int IsAnyCollected { get; set; }
        public string TStatus { get; set; }
    }
}
