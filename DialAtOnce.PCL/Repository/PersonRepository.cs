using System;
using SQLite.Net;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Xamarin3United.PCL
{
	public class PersonRepository
	{
		public PersonRepository ()
		{
			ISQLite sqlService = DependencyService.Get<ISQLite> ();

			if (sqlService.DatabaseExists () == false || sqlService.TableExists("Person") == false) {

				using (SQLiteConnection con = DependencyService.Get<ISQLite> ().GetConnection ()) {
					con.CreateTable<Person> ();
				}
			}
		}

		public Person Read(int id)
		{
			Person person = null;

			using(SQLiteConnection con = DependencyService.Get<ISQLite> ().GetConnection ())
			{
				person = con.Get<Person>(id);
			}

			return person;
		}

		public void Create(Person person)
		{
			using(SQLiteConnection con = DependencyService.Get<ISQLite> ().GetConnection ())
			{
				con.Insert (person);
			}
		}

		public List<Person> ReadAll()
		{
			List<Person> result = null;

			using (SQLiteConnection con = DependencyService.Get<ISQLite> ().GetConnection ()) 
			{
				result = con.Table<Person> ().ToList ();
			}

			return result;
		}

		public void Delete(int id)
		{
			using (SQLiteConnection con = DependencyService.Get<ISQLite> ().GetConnection ()) 
			{
				con.Delete<Person> (id);
			}
		}

		public void DeleteAll()
		{
			using (SQLiteConnection con = DependencyService.Get<ISQLite> ().GetConnection ()) 
			{
				con.DeleteAll<Person> ();
			}
		}
	}
}

