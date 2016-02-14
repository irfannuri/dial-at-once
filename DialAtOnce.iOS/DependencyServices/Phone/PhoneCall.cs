using System;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin3United.PCL.iOS;
using MonoTouch.Foundation;
using System.Linq;

[assembly: Dependency (typeof (PhoneCall))]
namespace Xamarin3United.PCL.iOS
{
	public class PhoneCall:IPhoneCall
	{
		public PhoneCall ()
		{
		}

		#region IPhoneCall implementation

		public void Call (string phoneNumber)
		{
			string rawnum = phoneNumber.Replace ("-", "").Replace ("(", "").Replace (")", "");
			rawnum = RemoveWhitespace (rawnum);

			NSUrl url = new NSUrl ("tel:"+ rawnum);
			UIApplication.SharedApplication.OpenUrl (url);
		}

		#endregion

		public static string RemoveWhitespace(string input)
		{
			return new string(input.ToCharArray()
				.Where(c => !Char.IsWhiteSpace(c))
				.ToArray());
		}
	}
}

