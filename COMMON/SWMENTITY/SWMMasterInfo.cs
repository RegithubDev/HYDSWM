using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COMMON.SWMENTITY
{
    public class ReasonInfo
    {
        [Key]
        public int ReasonId { get; set; }
        public string ReasonName { get; set; }
    }
    public class OwnerTypeInfo
    {
        [Key]
        public int OwnerTypeId { get; set; }
        public string OwnerType { get; set; }
    }
    public class PropertyTypeInfo
    {
        [Key]
        public int PropertyTypeId { get; set; }
        public string PropertyType { get; set; }
    }
    public class CircleInfo
    {
        [Key]
        public int CircleId { get; set; }
        public string CircleName { get; set; }
        public bool IsActive { get; set; }
    }

    public class RMNames
    {
        [Key]
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
       
    }
    public class WardInfo
    {
        [Key]
        public int WardId { get; set; }
        public string WardNo { get; set; }
        public string WardName { get; set; }
        public bool IsActive { get; set; }
        public int CirlceId { get; set; }
        public string CircleName { get; set; }
    }
    public class DepartmentInfo
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
    public class GenderInfo
    {
        [Key]
        public int GenderId { get; set; }
        public string GenderName { get; set; }
    }
    public class IdentityTypeInfo
    {
        [Key]
        public int IdentityTypeId { get; set; }
        public string IdentityName { get; set; }
    }
    public class DesignationInfo
    {
        [Key]
        public int DesignationId { get; set; }
        public string Designation { get; set; }
    }
    public class SectorInfo
    {
        [Key]
        public int SectorId { get; set; }
        public string SectorName { get; set; }
    }
    public class EmpTypeInfo
    {
        [Key]
        public int EmpTypeId { get; set; }
        public string EmpType { get; set; }
    }
    public class ShiftInfo
    {
        [Key]
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan ShiftSTime { get; set; }
        public TimeSpan ShiftETime { get; set; }
        public bool IsActive { get; set; }
        public int BeforBMin { get; set; }
        public int AfterBMin { get; set; }
        [NotMapped]
        public string CCode { get; set; }
        public string UserId { get; set; }
    }
}
