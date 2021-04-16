using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Models
{
    public class Machine
    {
        [Key] 
        public  int MachineId { get; set; }

        public string Type { get; set; }

        public DateTime InstallationDate { get; set; }
    }
}
