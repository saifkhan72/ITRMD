using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDWEB.Models
{
    public class ActivityLog
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int LogId { get; set; }
        public string? TableName { get; set; }
        public string? Activity { get; set; }
        public DateTime ActivityDate { get; set; }
        public string UserName { get; set; }
        public string? Detail { get; set; }
    }
}
