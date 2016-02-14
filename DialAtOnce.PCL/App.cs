using System;
using Xamarin.Forms;

namespace Xamarin3United.PCL
{
	public class App
	{
		public static Page GetMainPage ()
		{	
			DialMain dm = new DialMain ();

			//dm.Padding = new Thickness (10,Device.OnPlatform (20, 0, 0), 10, 5);

			return dm;

			//Entry etr;
			//etr.Keyboard = Keyboard.Telephone

//			var a = DependencyService.Get<IAddressBook> ().GetPeople ();
//
//			return new ContentPage { 
//				Content = new Label {
//					Text = "Hello, Forms !",
//					VerticalOptions = LayoutOptions.CenterAndExpand,
//					HorizontalOptions = LayoutOptions.CenterAndExpand,
//				},
//			};
		}
	}
}

