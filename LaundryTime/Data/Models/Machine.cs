﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Models
{
    public class Machine
    {
        [Key] 
        public string MachineId { get; set; }

        public string Type { get; set; }

        [Required]
        public string ModelNumber { get; set; }

        public DateTime InstallationDate { get; set; }
    }
}
