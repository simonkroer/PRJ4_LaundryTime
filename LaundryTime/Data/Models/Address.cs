using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public string StreetAddress { get; set; }
        public string Zipcode { get; set; }

        public string Country { get; set; }
    }
}
