using System;
using Xamarin.Forms;
using MonoTouch.Foundation;

[assembly: Dependency (typeof(Xamarin3United.PCL.iOS.UserPreferences))]
namespace Xamarin3United.PCL.iOS
{
	public class UserPreferences:IUserPreferences
	{
		public UserPreferences ()
		{
		}

		#region IUserPreferences implementation

		public void SetString (string key, string value)
		{
			NSUserDefaults.StandardUserDefaults.SetString (key, value);
		}

		public string GetString (string key)
		{
			return NSUserDefaults.StandardUserDefaults.StringForKey (key);
		}


		public void SetObject (string key, object value)
		{
			throw new NotImplementedException ();
		}

		public object GetObject (string key)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

