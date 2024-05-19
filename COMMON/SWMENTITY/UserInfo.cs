using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COMMON.SWMENTITY
{
    public class UserInfo
    {
        [Key]
        public string EmailId { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Pwd { get; set; }

    }
    [Table("tbl_User")]
    public class tbl_User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string FullName { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string EmailId { get; set; }


        [Required(ErrorMessage = "Required.")]
        public string Pwd { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastLogin { get; set; }
    }
    public class GUserInfo
    {
        [Key]
        public string EmailId { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Pwd { get; set; }
        public string LastLogin { get; set; }
        public string UserRole { get; set; }
    }
    
}
