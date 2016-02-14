using System;
using MonoTouch.Foundation;
using Xamarin.Forms;

[assembly: Dependency (typeof (Xamarin3United.PCL.iOS.StringBundle))]
namespace Xamarin3United.PCL.iOS
{
	public class StringBundle:IBundle
	{
		public StringBundle ()
		{
		}

		#region IBundle implementation

		public string LocalizedString (string key)
		{
			return NSBundle.MainBundle.LocalizedString (key, null);		
		}

		#endregion
	}
}

