using System;

namespace Xamarin3United.PCL
{
	public interface IUserPreferences
	{
		void SetString(string key, string value);
		string GetString(string key);

		void SetObject(string key, object value);
		object GetObject(string key);
	}
}

