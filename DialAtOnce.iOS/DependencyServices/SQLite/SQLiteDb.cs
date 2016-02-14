using System;
using Xamarin.Forms;
using System.IO;

[assembly: Dependency (typeof(Xamarin3United.PCL.iOS.SQLiteDb))]
namespace Xamarin3United.PCL.iOS
{
	public class SQLiteDb:ISQLite
	{
		public SQLiteDb ()
		{
		}

		protected string FilePath
		{
			get
			{
				var sqliteFilename = "PersonSQLite.db3";
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				var path = Path.Combine (libraryPath, sqliteFilename);
				return  path;
			}
		}

		public SQLite.Net.SQLiteConnection GetConnection ()
		{
			// Create the connection
			var plat = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS ();
			var conn = new SQLite.Net.SQLiteConnection (plat, FilePath);
			// Return the database connection

			var d = conn.GetTableInfo ("Person");

			var m = conn.Table<Person> ().Table;

			return conn;
		}

		public bool DatabaseExists ()
		{
			return System.IO.File.Exists (FilePath);
		}

		public bool TableExists(string tableName)
		{
			using (SQLite.Net.SQLiteConnection con = GetConnection ()) {

				var tableInfo = con.GetTableInfo (tableName);

				if (tableInfo != null && tableInfo.Count > 0)
					return true;
				else
					return false;
			}
		}
	}
}

