using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDWEB.Models
{
    public class BankTbl
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int BankId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public int StatusId { get; set; }


        [ForeignKey("StatusId")]
        public virtual StatusTbl StatusTbl { get; set; }



    }
}
