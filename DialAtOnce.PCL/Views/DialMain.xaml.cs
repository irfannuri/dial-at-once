using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using System.ComponentModel;
using System.Resources;
using System.Threading.Tasks;

namespace Xamarin3United.PCL
{
	public partial class DialMain : TabbedPage
	{
		List<Person> people;
		List<KeyValuePair<string, Person>> dictionary;
		T9Encoder encoder;
		string filter = null;
		DialSearchEntry searchBar;

		public DialMain ()
		{
			InitializeComponent ();

			searchBar = new DialSearchEntry () {
				Keyboard = Keyboard.Telephone,
				BackgroundColor = Color.White,
				Placeholder = DependencyService.Get<IBundle> ().LocalizedString ("SearchEntryPlaceHolder"),
				TextColor = Color.FromHex ("#494c53"),
					
			};

			this.stack.Children.Insert (0, searchBar); 

			DependencyService.Get<IAddressBook> ().CheckAccessPermission (AccessControl);
		}

		public IEnumerable<Person> SearchResult {
			get {
				if (string.IsNullOrEmpty (filter))
					return people;
				else {
					return people.Where (p => p.NumberIndex.Contains (filter));
				}
			}
		}

		void AccessControl (bool granted)
		{
			if (granted)
				LoadPeople ();
			else
				return;
		}

		async void LoadPeople ()
		{
			dictionary = new List<KeyValuePair<string, Person>> ();
			encoder = new T9Encoder ();

			searchBar.TextChanged += OnSearchBarTextChanged;

			people = new PersonRepository ().ReadAll ();

			people.Sort ((Person p1, Person p2) => {
				return p1.DisplayName.CompareTo (p2.DisplayName);
			});

			resultList.BackgroundColor = Color.FromHex ("#cfd2d6");
			//resultList.VerticalOptions = LayoutOptions.Center;

			resultList.ItemTemplate = new DataTemplate (() => {
				// Create views with bindings for displaying each property.
				Label nameLabel = new Label ();
				nameLabel.SetBinding (Label.TextProperty, "DisplayName");
				nameLabel.Font = Font.OfSize ("GillSans-Bold", NamedSize.Medium);
				nameLabel.TextColor = Color.White;
				nameLabel.VerticalOptions = LayoutOptions.Center;

				Label detailsLabel = new Label ();
				detailsLabel.SetBinding (Label.TextProperty, "Details");
				detailsLabel.VerticalOptions = LayoutOptions.Center;
				detailsLabel.Font = Font.OfSize ("GillSans", NamedSize.Small);
				detailsLabel.TextColor = Color.FromHex ("#78b9b5");

				return new ContactViewCell { 
					View =
							new StackLayout {
						Spacing = 0,
						Padding = new Thickness (5, 3, 1, 3),
						Orientation = StackOrientation.Vertical,
						BackgroundColor = Color.FromHex ("#494c53"),
						VerticalOptions = LayoutOptions.FillAndExpand,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Children = {
							//img,
							nameLabel,
							detailsLabel
						}
					}
				};
			});

			resultList.ItemTapped += OnItemTapped;

			searchBar.Focus ();

			IAddressBook addressBook = DependencyService.Get<IAddressBook> ();
			people = addressBook.GetPeople ();

			resultList.ItemsSource = SearchResult;
		}

		async Task<List<Person>> GetRecentPeopleData ()
		{
			List<Person> result = await AddressBookUpdate ();
			return result;
		}

		async Task<List<Person>> AddressBookUpdate ()
		{
			IAddressBook addressBook = DependencyService.Get<IAddressBook> ();
			List<Person> bookList = addressBook.GetPeople ();

			string lastUpdatedString = DependencyService.Get<IUserPreferences> ().GetString ("LastUpdated");
			DateTime lastUpdated = DateTime.FromBinary (Convert.ToInt64 (lastUpdatedString));
			PersonRepository repo = new PersonRepository ();
			List<Person> result = null;

			if (lastUpdated <= DateTime.MinValue) {
				repo.DeleteAll ();

				foreach (Person p in bookList)
					repo.Create (p);

				result = bookList;
			}

			DependencyService.Get<IUserPreferences> ().SetString ("LastUpdated", DateTime.Now.ToBinary ().ToString ());

			return result;
		}

		void OnSearchBarTextChanged (object sender, TextChangedEventArgs args)
		{
			filter = GetFilter (args.NewTextValue);
			resultList.ItemsSource = SearchResult;
		}

		async void OnItemTapped (object sender, ItemTappedEventArgs e)
		{
			Person person = e.Item as Person;

			if (string.IsNullOrEmpty (person.PhoneData))
				return;

			List<PhoneNumber> phoneNumbers = new List<PhoneNumber> ();
			string[] pNumbers = person.PhoneData.TrimEnd (';').Split (';');

			foreach (string number in pNumbers) {
				string[] values = number.Split (',');

				PhoneNumberType type = (PhoneNumberType)Convert.ToInt32 (values [0]);

				phoneNumbers.Add (new PhoneNumber (){ Type = type, Number = values [1] });
			}

			if (phoneNumbers.Count == 1) {
				DependencyService.Get<IPhoneCall> ().Call (phoneNumbers [0].Number);
			} else if (phoneNumbers.Count > 1) {
				IEnumerable<string> numbers = phoneNumbers.Select (s => s.Number + " ['" + s.Type.ToString () + "']");
				string action = await DisplayActionSheet ("Select a number", "Cancel", null, numbers.ToArray ());

				if (action.Equals ("Cancel"))
					return;

				string no = action.Remove (action.IndexOf ("['")).Trim ();
				DependencyService.Get<IPhoneCall> ().Call (no);
			}
		}

		public string GetFilter (string value)
		{
			if (string.IsNullOrEmpty (value) == false) {
				List<char> newFilter = new List<char> ();

				foreach (char c in value) {
					if (Char.IsDigit (c) == false || c == '0') {
						newFilter.Add ('1');
					} else {
						newFilter.Add (c);
					}
				}

				return new string (newFilter.ToArray ());
			} else {
				return value;				
			}
		}
	}
}

