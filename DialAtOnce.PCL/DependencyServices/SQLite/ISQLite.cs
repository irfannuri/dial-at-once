using System;
using SQLite.Net;

namespace Xamarin3United.PCL
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection ();

		bool DatabaseExists ();

		bool TableExists(string tableName);
	}
}

