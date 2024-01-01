using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDWEB.Models
{
    public class DepartmentTbl
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int DepartmentId { get; set; }

        [Required]
        public string Name { get; set; }

        public int BankId { get; set; }
        public int StatusId { get; set; }



        [ForeignKey("StatusId")]
        public virtual StatusTbl StatusTbl { get; set; }


        [ForeignKey("BankId")]
        public virtual BankTbl BankTbl { get; set; }



    }
}
