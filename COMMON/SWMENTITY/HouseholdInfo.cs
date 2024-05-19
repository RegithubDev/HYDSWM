using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COMMON.SWMENTITY
{
    public class HouseholdInfo
    {

        [Key]
        public int? HouseId { get; set; }


        public string UHouseId { get; set; }
        [Required(ErrorMessage = "Owner Name Is Required")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Address Is Required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Ward No Is Required")]
        public string WardNo { get; set; }
        public string ContactNo { get; set; }
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Lattitude Is Required")]
        public string Lat { get; set; }

        [Required(ErrorMessage = "Longitude Is Required")]
        public string Lng { get; set; }
        public string CircleId { get; set; }
        public bool IsActive { get; set; }
        public string OwnerType { get; set; }
        public int PropertyTypeId { get; set; }
        public string IdentityTypeId { get; set; }
        public string IdentityNo { get; set; }
        [NotMapped]
        public string UserId { get; set; }
        [NotMapped]
        public string CCode { get; set; }
        public int CollectionType { get; set; }
        public string PropertyId { get; set; }
        public int SectorId { get; set; }
    }
    public class HouseHold_Paging
    {

        private int? _A;
        [Key]
        public int? HouseId
        {
            get
            {
                return _A;
            }
            set
            {
                _A = value;
            }
        }

        public int? TotalCount { get; set; }
        [NotMapped]
        public string EncHouseId
        {
            get
            {
                return CommonHelper.Encrypt(_A.ToString());

            }
            //set
            //{

            //    value = CommonHelper.Encrypt(this.OwnerName.ToString());
            //}
        }
        public string UHouseId { get; set; }
        [Required(ErrorMessage = "Owner Name Is Required")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Address Is Required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Ward No Is Required")]
        public string WardNo { get; set; }
        public string ContactNo { get; set; }
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Lattitude Is Required")]
        public string Lat { get; set; }

        [Required(ErrorMessage = "Longitude Is Required")]
        public string Lng { get; set; }
        public string CircleName { get; set; }
        public bool IsActive { get; set; }
        public string OwnerType { get; set; }
        public string PropertyType { get; set; }
    }
    public class HouseQrCodeInfo
    {
        [Key]
        public string QrCode { get; set; }
        public int TypeId { get; set; }
        public string GType { get; set; }
    }
}
