using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data
{
    public class SmsAccount
    {
     public string AccountSid { get; set; }
     public string AuthToken { get; set; }
     public string PhoneNumber { get; set; }
    }
}
