using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt4.Models
{
	public class Maskine
	{
		[Key]
		public int Id { get; set; }

		[Display(Name="Maskine Navn")]
		[Required]
		[MaxLength(20)]
		public string Name { get; set; }
	}
}
