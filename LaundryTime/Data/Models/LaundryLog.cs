using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Models
{
    public class LaundryLog
    {
        public int LogId { get; set; }
        public string LogInfo { get; set; }

        public DateTime LogDate { get; set; }
    }
}
