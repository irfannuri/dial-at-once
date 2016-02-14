using System;
using Xamarin.Forms;
using Xamarin3United.PCL;
using Xamarin3United.PCL.iOS;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;

[assembly: ExportRenderer (typeof(DialSearchEntry), typeof(DialSearchEntryRenderer))]
namespace Xamarin3United.PCL.iOS
{
	public class DialSearchEntryRenderer: EntryRenderer, IUITextFieldDelegate
	{
		public DialSearchEntryRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> eargs)
		{
			base.OnElementChanged (eargs);

			var nativeTextField = Control as UITextField;

			nativeTextField.KeyboardAppearance = UIKeyboardAppearance.Dark;

			UIToolbar toolbar = new UIToolbar (new RectangleF (0.0f, 0.0f, nativeTextField.Frame.Size.Width, 44.0f));

			toolbar.TintColor = UIColor.White;
			toolbar.BarStyle = UIBarStyle.Black;

			toolbar.Translucent = true;

			UIBarButtonItem callButton = new UIBarButtonItem (NSBundle.MainBundle.LocalizedString ("CallButton", null),
				               UIBarButtonItemStyle.Bordered, AddBarButtonText);

			UIBarButtonItem cancelButton = new UIBarButtonItem(NSBundle.MainBundle.LocalizedString ("Hide", null),
				UIBarButtonItemStyle.Bordered, HideButtonClick);

			UIImage image = UIImage.FromBundle("down-arrow-icon");

			cancelButton.Image = image; 


			toolbar.Items = new UIBarButtonItem[] {
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
				callButton,
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
				cancelButton
			};

			toolbar.ContentMode = UIViewContentMode.Center;

			nativeTextField.InputAccessoryView = toolbar;
		}

		public void HideButtonClick(object sender,  EventArgs e)
		{
			var nativeTextField = Control as UITextField;
			nativeTextField.EndEditing (true);
		}

		public void AddBarButtonText (object sender, EventArgs e)
		{
			var nativeTextField = Control as UITextField;

			NSUrl url = new NSUrl ("tel:" + nativeTextField.Text);
			UIApplication.SharedApplication.OpenUrl (url);
		}
	}
	
}

