using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Utilities
{
    public class Report:IReport
    {
        public string Format { get; set; }
        public string FileName { get; set; }
        public MemoryStream Content { get; set; }
    }
}
