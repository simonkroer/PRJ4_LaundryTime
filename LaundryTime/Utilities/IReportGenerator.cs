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
        public IReport GenerateReport<T>(ICollection<T> collection, string format = "text/json", string filename = "MyUsersReport.json");

        IReport GenerateMyUsersReport(List<LaundryUser> users, string format = "text/json", string filename = "MyUsersReport.json");

        IReport GenerateMyMachinesReport(List<Machine> machines, string format = "text/json",
            string filename = "MyMachinesReport.json");


    }
}
