using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LaundryTime.Utilities
{
    public class ReportGenerator: IReportGenerator
    {
        public IReport _report { get; set; }
        public IReport GenerateMyUsersReport(List<LaundryUser> users, string format = "text/json", string filename = "MyUsersReport.json")
        {
            var builder = new StringBuilder();

            foreach (var user in users) // Alternative: Build json object manually
            {
                var builder2 = new StringBuilder();

                foreach (var log in user.LaundryHistory)
                {
                    log.LaundryUser = null;
                    builder2.Append(log);
                }

                builder.Append(JsonConvert.SerializeObject(user));
                builder.Append(JsonConvert.SerializeObject(builder2));
            }

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
            var content = new MemoryStream(bytes);

            _report = new Report()
            {
                Content = content, 
                FileName = filename, 
                Format = format
            };

            return _report;
        }

        public Task<IActionResult> GenerateMyMachinesReport()
        {
            throw new NotImplementedException();
        }
    }
}
