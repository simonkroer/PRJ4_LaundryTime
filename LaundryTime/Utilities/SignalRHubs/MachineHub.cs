using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LaundryTime.Utilities.SignalRHubs
{
    public class MachineHub: Hub
    {
        public async Task StatusChanged()
        {
            await Clients.All.SendAsync("Status Changed");
        }
    }
}
