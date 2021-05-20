using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Models
{
    public class MessageToUserAdmin
    {
        [Key]
        public int MessageId { get; set; }
        public DateTime SendDate { get; set; }
        public string MessageInfo { get; set; }
        [ForeignKey("LaundryUserId")]
        public LaundryUser LaundryUser { get; set; }
    }
}
