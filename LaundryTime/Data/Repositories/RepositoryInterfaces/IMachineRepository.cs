﻿using LaundryTime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Repositories.RepositoryInterfaces
{
    public interface IMachineRepository : IRepository<Machine>
    {
        List<Machine> GetAllMachines();
        Machine GetSingleMachine(int id);
        void AddMachine(Machine machine);
        int GetNumberOfMachines();
        public bool MachineExist(string number);
    }
}
