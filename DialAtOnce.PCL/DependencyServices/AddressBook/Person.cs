using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using SQLite.Net.Attributes;

namespace Xamarin3United.PCL
{
	[Table ("Person")]
	public class Person
	{
		public Person ()
		{
			Phones = new List<PhoneNumber> ();
		}

		public Person (int id, string firstName, string middleName, string lastName) : this ()
		{
			this.Id = id;
			this.FirstName = firstName;
			this.MiddleName = middleName;
			this.LastName = lastName;
			this.NumberIndex = ComputeNumberIndex (this.DisplayName);
		}

		private string details = null;

		[PrimaryKey]
		public int Id {
			get;
			set;
		}

		public string FirstName {
			get;
			set;
		}

		public string MiddleName {
			get;
			set;
		}

		public string LastName {
			get;
			set;
		}

		public string NumberIndex {
			get;
			private set;
		}

		[Ignore]
		public List<PhoneNumber> Phones {
			get;
			set;
		}

		[Ignore]
		public ImageSource Image { 
			get; 
			set; 
		}

		public string NickName {
			get;
			set;
		}

		public string Organization {
			get;
			set;
		}

		public string Notes {
			get;
			set;
		}

		public string DetailData {
			get;
			set;
		}

		public string PhoneData {
			get;
			set;
		}

		[Ignore]
		public string Details {
			get {

				if (details == null) {

					if (DetailData == null) {

						string phone = RemoveCharacters (Phones [0].Number);

						if (Phones.Count > 1) {
							StringBuilder bld = new StringBuilder ();

							for (int i = 0; i < Phones.Count; i++) {
								string phn = RemoveCharacters (Phones [i].Number);						
								bld.Append (string.Format ("{0}", i == 0 ? phn : " / " + phn));
							}

							details = bld.ToString ();

						} else {
							if (string.IsNullOrWhiteSpace (Organization))
								details = phone;
						else
								details = string.Format ("{0}, {1}", Organization, phone).Trim (',');
						}
					
						DetailData = details;
					} else
						details = DetailData;
				}

				return details;
			}
		}

		[Ignore]
		public bool HasImage {
			get{ return Image != null; }
		}

		[Ignore]
		public bool HasNotImage {
			get{ return Image == null; }
		}

		private string displayName;

		public string DisplayName {
			get {
				if (displayName == null) {
					if (string.IsNullOrWhiteSpace (MiddleName)) {
						displayName = string.Format ("{0} {1}", FirstName, LastName).Trim ();
					} else {
						displayName = string.Format ("{0} {1} {2}", FirstName, MiddleName, LastName).Trim ();
					}
				}

				return displayName.TrimStart ();
			}
		}

		private DateTime modificationDate;

		public DateTime ModificationDate {
			get{ return modificationDate; }
			set{ modificationDate = value; }
		}

		private static string ComputeNumberIndex (string text)
		{
			return Mapping.Current.TranslateToDigits (text);
		}

		public static string RemoveCharacters (string input)
		{
			return Regex.Replace (input, @"^\+90|^090|^90|^0", string.Empty);
		}
	}
}

