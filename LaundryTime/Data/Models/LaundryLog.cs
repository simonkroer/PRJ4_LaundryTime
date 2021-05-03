using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Models
{
    public class LaundryLog
    {
        [Key]
        public int LogId { get; set; }
        public string LogInfo { get; set; }

        public DateTime LogDate { get; set; }
        [ForeignKey("LaundryUserId")]
        public LaundryUser LaundryUser { get; set; }
    }
}
