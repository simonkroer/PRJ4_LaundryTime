using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt4.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork : IDisposable
	{
		IMaskineRepository Maskine { get; }
		IApplicationUserRepository ApplicationUser { get; }
		ISP_Call SP_Call { get; }

		void Save();
	}
}
