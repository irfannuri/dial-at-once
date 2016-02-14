using System;
using System.Collections.Generic;


namespace Xamarin3United.PCL
{
	public interface IAddressBook
	{
		List<Person> GetPeople();
		void CheckAccessPermission (Action<bool> onResult);
		Action AddressBookChanged { get; set; }
	}
}

