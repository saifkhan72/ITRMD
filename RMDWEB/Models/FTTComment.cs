using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDWEB.Models
{
    public class FTTComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int fttcommentid { get; set; }
        public string comment { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public int TID { get; set; }

        public int StatusId { get; set; }


        [ForeignKey("StatusId")]
        public virtual StatusTbl StatusTbl { get; set; }





    }
}
