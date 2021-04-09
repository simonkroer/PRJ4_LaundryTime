﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTimeWebAppWithIdentity.Models
{
    public class Machine
    {
        [Key]
        public int MachineId { get; set; }

        public string MachineType { get; set; }

        public DateTime InstallationDateTime { get; set; }

        public UserAdmin owner { get; set; }
    }
}