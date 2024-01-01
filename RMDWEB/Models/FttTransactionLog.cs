using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RMDWEB.Models
{
    public class FttTransactionLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int LID { get; set; }
        public int TID { get; set; }
        public string Comment { get; set; }
        public int? DepartmentId { get; set; }
        public int BankLetterId { get; set; }
        public DateTime TTDate { get; set; }
        public int TTNumber { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public float TTAmount { get; set; }
        public int StatusId { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Required]
        public string InvoiceContractNo { get; set; }
        public string SenderName { get; set; }
        public string BenBank { get; set; }
        public string PurposeTransaction { get; set; }
        public string BenCountry { get; set; }
        public string BenCompany { get; set; }
        public int CurrencyId { get; set; }

    }
}
