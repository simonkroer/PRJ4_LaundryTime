using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Projekt4.Models
{
	public class ApplicationUser : IdentityUser
	{

		[Required]
		public string Name { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string BoligNummer { get; set; }

		[NotMapped]
		public String Role { get; set; }
	}
}
