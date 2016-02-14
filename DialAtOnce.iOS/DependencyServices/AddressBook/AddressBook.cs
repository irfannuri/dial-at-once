using System;
using MonoTouch.AddressBook;
using MonoTouch.Foundation;
using System.Collections.Generic;
using System.Linq;
using Xamarin3United.PCL.iOS;
using Xamarin.Forms;
using Xamarin.Contacts;
using System.Threading.Tasks;
using System.Text;

[assembly: Dependency (typeof (Xamarin3United.PCL.iOS.AddressBook))]
namespace Xamarin3United.PCL.iOS
{
	public class AddressBook:IAddressBook
	{
		public AddressBook ()
		{
		}

		private Action addressBookChanged;
		public Action AddressBookChanged 
		{
			get {
				return addressBookChanged;
			}
			set {
				addressBookChanged = value;
			}
		}

		#region IAddressBook implementation

		public List<Person> GetPeople ()
		{
			NSError error;
			List<Person> result = new List<Person> ();

			Xamarin.Contacts.AddressBook book = new Xamarin.Contacts.AddressBook ();

			ABAddressBook nativeBook = ABAddressBook.Create (out error);
			nativeBook.ExternalChange += BookChanged;

			foreach (Xamarin.Contacts.Contact c in book) {

				if (c.Phones.Count() > 0) {
					Person p = new Person (Convert.ToInt32(c.Id), c.FirstName, c.MiddleName, c.LastName);
				
					p.ModificationDate = nativeBook.GetPerson(p.Id).ModificationDate;

					p.NickName = c.Nickname;
					if (c.Organizations.Count () > 0) {
						p.Organization = c.Organizations.ElementAt (0).Name;
					}

					p.Notes = String.Concat (c.Notes.Select (n => n.Contents));

					StringBuilder sb = new StringBuilder ();

					foreach (Phone phone in c.Phones) {
						PhoneNumber pnb = new PhoneNumber (){ Type = (PhoneNumberType)((int)phone.Type), Number = phone.Number }; 
						p.Phones.Add (pnb);
						sb.AppendFormat ("{0},{1};", (int)pnb.Type, pnb.Number);
					}
					p.DetailData = p.Details;
					p.PhoneData = sb.ToString ();

					result.Add (p);
				}
			}



			return result;
		}

		public void BookChanged(object sender, ExternalChangeEventArgs e)
		{
			List<int> changes = new List<int> ();

			DateTime dt = new DateTime (2014, 12, 9);

			foreach (ABRecord rec in e.AddressBook) {
			
				ABPerson per = rec as ABPerson;

				if (per != null) {
				
					if (per.CreationDate > dt) {
						changes.Add (per.Id);
					}
				}
			}

		}

		public void CheckAccessPermission (Action<bool> onResult)
		{
			Xamarin.Contacts.AddressBook book = new Xamarin.Contacts.AddressBook ();
			book.RequestPermission().ContinueWith (t => {
				if (!t.Result) {
					onResult(false);
				}
				else onResult(true); 

			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		#endregion
	}
}

