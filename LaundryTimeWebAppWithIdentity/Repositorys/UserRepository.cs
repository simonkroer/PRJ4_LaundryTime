using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTimeWebAppWithIdentity.Data;
using LaundryTimeWebAppWithIdentity.Models.Calender;
using Microsoft.EntityFrameworkCore;

namespace LaundryTimeWebAppWithIdentity.Repositorys
{
    public class UserRepository : Repository<UserModel>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public IEnumerable<UserModel> GetUsersReservedBookings()
        {
            return _context.Users.Include(u=>u.reservedBooking).ToList();
        }
    }
}
