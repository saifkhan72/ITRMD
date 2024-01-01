using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDWEB.Models
{
    public class FTTCommentLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int fttcomentidLogId{ get; set; }
        public int fttcommentid { get; set; }
        public string comment { get; set; }
        [Required]
        public DateTime Created { get; set; }

        public int? StatusId { get; set; }
        public string UserId { get; set; }
        public int TID { get; set; }
    }
}
