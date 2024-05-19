using System.ComponentModel.DataAnnotations;

namespace COMMON.SWMENTITY
{
    public class QRScannedInfo
    {
        [Key]
        public int CTId { get; set; }
        public string UHouseId { get; set; }
        public string OwnerName { get; set; }
        public string Address { get; set; }
        public string WardNo { get; set; }
        public string ContactNo { get; set; }
        public string DImei { get; set; }
        public string SubmitBy { get; set; }
        public string TDate { get; set; }
        public string CLat { get; set; }
        public string CLNG { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string OwnerType { get; set; }
        public string PropertyType { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public int TotalCount { get; set; }
    }
}
