using System;

namespace Xamarin3United.PCL
{
	public class PhoneNumber
	{
		public PhoneNumber ()
		{
		}

		public PhoneNumberType Type {
			get;
			set;
		}

		public string Number {
			get;
			set;
		}
	}

	public enum PhoneNumberType
	{
		Home,
		HomeFax,
		Work,
		WorkFax,
		Pager,
		Mobile,
		Other
	}

}

