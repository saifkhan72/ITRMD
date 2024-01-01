using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using RMDWEB.Models;

namespace RMDWEB.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string PhoneNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public int? UserId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Designation { get; set; }
        public string? FatherName { get; set; }
        public int? Dab { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string? IpAddress { get; set; }
        public int DepartmentId { get; set; }
        public int StatusId { get; set; }
        public int? BankId { get; set; }

        [ForeignKey("StatusId")]
        public virtual StatusTbl StatusTbl { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual DepartmentTbl DepartmentTbl { get; set; }


        [ForeignKey("BankId")]
        public virtual BankTbl BankTbl { get; set; }


    }
}
