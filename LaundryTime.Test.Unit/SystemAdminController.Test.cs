using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryTime.Test.Unit
{
    class SystemAdminControllerTest
    {
        protected ApplicationDbContext _context { get; set; }
        protected SystemAdminController _uut;

        public SystemAdminControllerTest()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);

            Seed();

            _uut = new SystemAdminController()

            _uut._systemAdminViewModel = Substitute.For<SystemAdminViewModel>();
        }

    }
}
