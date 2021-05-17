using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaundryTime.Utilities
{
    public interface IReportGenerator
    {
        public IReport GenerateMyUsersReport(List<LaundryUser> users, string format = "text/json", string filename = "MyUsersReport.json");

        public Task<IActionResult> GenerateMyMachinesReport();


    }
}
