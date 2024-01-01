using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDWEB.Models
{
    public class CurrencyTbl
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int CurrencyId { get; set; }
        public string CurName { get; set; }
        public string CurNameDa { get; set; }
        public string CurSign { get; set; }
        public string CurCountry { get; set; }
        public int StatusId { get; set; }


        [ForeignKey("StatusId")]
        public virtual StatusTbl StatusTbl { get; set; }

  

    }

}
