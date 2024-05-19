using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace COMMON.GENERIC
{
   public class EmployeeInfo_Paging
    {
        private int _A;
        [Key]
        public int EmployeeId
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
        [NotMapped]
        public string EncEmpId
        {
            get
            {
                return CommonHelper.Encrypt(_A.ToString());

            }
           
        }
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string GuardianName { get; set; }
        public string ContactNo { get; set; }
        public bool IsActive { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string IdentityType { get; set; }
        public string IdentityNo { get; set; }
        public string Designation { get; set; }
        public string CircleName { get; set; }
        public string WardNo { get; set; }
        public string SectorName { get; set; }
        public string RMName { get; set; }
        public int TotalCount { get; set; }
    }
    public class EmployeeInfo
    {
        [Key]
        public int EmployeeId { get; set; }
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string GuardianName { get; set; }
        public string ContactNo { get; set; }
        public bool IsActive { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string IdentityTypeId { get; set; }
        public int GenderId { get; set; }
        public string IdentityNo { get; set; }
        public int DepartmentId { get; set; }
        public string DesignationId { get; set; }
        public string CircleId { get; set; }
        public int RMId { get; set; }
        public string WardId { get; set; }
        public string SectorId { get; set; }
        public string CCode { get; set; }
        [NotMapped]
        public string CreatedBy { get; set; }
        public int EmpTypeId { get; set; }
        public int ShiftId { get; set; }
        public string TempId { get; set; }
        public string EmpId { get; set; }
    }
}
