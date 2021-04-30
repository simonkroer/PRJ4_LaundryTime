using LaundryTime.Data.Models;
using LaundryTime.Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Repositories
{
    public class MachineRepository : Repository<Machine>, IMachineRepository
    {
        public ApplicationDbContext context
        {
            get { return Context as ApplicationDbContext; }
        }
        public MachineRepository(ApplicationDbContext context):base(context) { }
        public List<Machine> GetAllMachines()
        {
            //skal der ikke være noget med hvilke tider der er på maskinerne? Fremmednøgle til de bookede tider?
            return context.Machines.ToList();
        }

        public Machine GetSingleMachine(int id)
        {
            //Igen, noget med tider?
            return context.Machines
                .SingleOrDefault(m => m.MachineId == id);
        }

        public void AddMachine(Machine machine)
        {
            context.Machines.Add(machine);
        }

        public void DelMachine(int id)
        {
            var machineToDel = context.Machines.SingleOrDefault(i => i.MachineId == id);
            
            if(machineToDel != null)
                context.Machines.Remove(machineToDel);
        }

        public int GetNumberOfMachines()
        {
            return context.Machines.Count();
        }

        public string GetTypeOfMachine(int id)
        {
            var machine = context.Machines.SingleOrDefault(i => i.MachineId == id);

            if (machine != null)
                return machine.Type;
            
            else
                return "";
        }

        public bool MachineExist(string number)
        {
            return context.Machines.Any(i => i.ModelNumber == number);
        }
    }
}
