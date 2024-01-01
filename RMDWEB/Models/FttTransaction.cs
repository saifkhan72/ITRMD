using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RMDWEB.Models
{
    public class FTTTransaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int TID { get; set; }
        public string Comment { get; set; }
        [Required]
        public int BankLetterId { get; set; }
        public DateTime TTDate { get; set; } = DateTime.Now;
        public int TTNumber { get; set; }


        [Required]
        public string InvoiceContractNo { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public float TTAmount { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Required]
        public string SenderName { get; set; }
        [Required]
        public string BenBank { get; set; }
        [Required]
        public string PurposeTransaction { get; set; }
        [Required]
        public string BenCountry { get; set; }
        [Required]
        public string BenCompany { get; set; }
        public string UserId { get; set; }


        public int CurrencyId { get; set; }
        public int StatusId { get; set; }
        public int DepartmentId { get; set; }
        public int? BankId { get; set; }



        [ForeignKey("CurrencyId")]
        public virtual CurrencyTbl CurrencyTbl { get; set; }



        [ForeignKey("StatusId")]
        public virtual StatusTbl StatusTbl { get; set; }



        [ForeignKey("DepartmentId")]
        public virtual DepartmentTbl DepartmentTbl { get; set; }




        [ForeignKey("BankId")]
        public virtual BankTbl BankTbl { get; set; }


    }

}
