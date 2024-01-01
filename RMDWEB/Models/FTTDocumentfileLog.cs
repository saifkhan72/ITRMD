using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RMDWEB.Models
{
    public class FTTDocumentfileLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int DocIdLog{ get; set; }
        public int DocId { get; set; }
        public string Title { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        [Required]
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        [Required]
        public int TID { get; set; }
        [Column(TypeName = "image")]
        public byte[]? DocImage { get; set; }
        [Required]
        public string DocImagePath { get; set; }
        public int? StatusId { get; set; }

    }
}
