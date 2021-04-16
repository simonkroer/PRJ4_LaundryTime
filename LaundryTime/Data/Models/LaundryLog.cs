using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Models
{
    public class LaundryLog
    {
        [Key]
        public string LogId { get; set; }
        public string LogInfo { get; set; }

        public DateTime LogDate { get; set; }
    }
}
