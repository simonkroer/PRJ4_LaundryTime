﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LaundryTime.Data.Models
{
    public class LaundryUser: IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public string PaymentMethod { get; set; }

        [ForeignKey("Id")]
        public UserAdmin Administrator { get; set; }

        [ForeignKey("LogId")]
        public List<LaundryLog> LayndryHistory { get; set; }

        [ForeignKey("AddressId")]
        public Address Address { get; set; }

        public bool ActiveUser { get; set; }

        public decimal FinancialBalance { get; set; }

        public DateTime PaymentDueDate { get; set; }
    }
}
