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
	public class MaskineRepository : Repository<Maskine>, IMaskineRepository
	{
		private readonly ApplicationDbContext _db;

		public MaskineRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Maskine maskine)
		{
			var objFromDb = _db.Maskiner.FirstOrDefault(s => s.Id == maskine.Id);
			if (objFromDb != null)
			{
				objFromDb.Name = maskine.Name;
				
			}
		}
	}
}
