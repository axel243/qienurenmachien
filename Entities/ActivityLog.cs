using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Entities
{
    public class ActivityLog
    {
        [Key]
        public int LogId { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser applicationUser { get; set; }
        public string Id { get; set; }
        public string Activity { get; set; }
        public string Comment { get; set; }
        public string Timestamp { get; set; }
    }
}
