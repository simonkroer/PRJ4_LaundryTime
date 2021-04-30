using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Models
{
    public class Machine
    {
        [Key]
        [Display(Name = "Machine ID")]
        public int MachineId { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Model Number")]
        public string ModelNumber { get; set; }

        [ForeignKey(name: "UserAdminId")]
        public string UserAdminId { get; set; }

        public UserAdmin UserAdmin { get; set; }

        [Display(Name = "Installation Date")]
        public DateTime InstallationDate { get; set; }
    }
}
