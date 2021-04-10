using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt4.DataAccess.Data;
using Projekt4.DataAccess.Repository.IRepository;
using Projekt4.Models;

namespace Projekt4.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _db;

		public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			Maskine = new MaskineRepository(_db);
			ApplicationUser = new ApplicationUserRepository(_db);
			SP_Call = new SP_Call(_db);
		}

		public IApplicationUserRepository ApplicationUser { get; private set; }
		public IMaskineRepository Maskine { get; private set; }
		public ISP_Call SP_Call { get; private set; }

		public void Dispose()
		{
			_db.Dispose();
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
