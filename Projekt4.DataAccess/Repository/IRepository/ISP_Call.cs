using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt4.DataAccess.Repository.IRepository
{
	public interface ISP_Call : IDisposable
	{
		T Single<T>(string procedureName, DynamicParameters param = null);

		void Execute(string procedureName, DynamicParameters param = null);

		T OneRcord<T>(string procedureName, DynamicParameters param = null);

		IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null);

		Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null);
	}
}

#region Beskrivelse

// Denne klasse bruges til at styre/kalde, det som er i databasen.


//Task Single<T>(string procedureName, DynamicParameters param = null);
// Når man skal returnere en value

//void Execute(string procedureName, DynamicParameters param = null);
// Når man vil execute i databasen uden at hente noget

//void OneRcord<T>(string procedureName, DynamicParameters param = null);
// Når man vil have en hel række eller record

//IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null);
//Når man vil hente alle rekker

//Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null);
//Når man vil hente to table

#endregion
