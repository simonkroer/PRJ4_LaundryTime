using LaundryTime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Repositories.RepositoryInterfaces
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public ApplicationDbContext context
        {
            get { return Context as ApplicationDbContext; }
        }
        public AddressRepository(ApplicationDbContext context) : base(context) { }
        public void AddAddress(Address address)
        {
            Context.Addresses.Add(address);
        }

        public Address GetSingleAddress(int id)
        {
            return Context.Addresses
                .SingleOrDefault(a => a.AddressId == id);
        }

        public List<Address> GetAllAdresses()
        {
            return new List<Address>(Context.Addresses
                .ToList());
        }
    }
}
