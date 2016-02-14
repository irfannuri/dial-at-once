using System;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

namespace Xamarin3United.PCL
{
	public abstract class Mapping
	{
		private string name;
		private Dictionary<char, IEnumerable<char>> directMappings = new Dictionary<char, IEnumerable<char>> ();
		private Dictionary<char, char> reverseMappings = new Dictionary<char, char>();

		private static Mapping current;

		public static Mapping Current {
			get {
				if (current == null) {
					current = new ES202130_111_LanguageIndependent_Latin ();
				}

				return current;
			}
			internal set {
				current = value;
			}
		}


		protected Mapping(string name) {
			this.name = name;

			Populate ();
		}

		public string Name {
			get { return this.name; }
		}

		protected abstract void Populate();

		protected void Add(char digit, IEnumerable<string> mappings) {
			List<char> translations = new List<char> ();

			foreach (string mappedUnicodeChar in mappings) {
				char lowerChar = (char)UInt16.Parse (mappedUnicodeChar.Substring(2), System.Globalization.NumberStyles.HexNumber);
				char upperChar = Char.ToUpper (lowerChar);

				translations.Add (lowerChar);
				reverseMappings [lowerChar] = digit;
				if (lowerChar != upperChar) {
					translations.Add (upperChar);
					reverseMappings[upperChar] = digit;
				}
			}

			directMappings.Add (digit, translations);
		}

		public char TranslateToDigit(char c) {
			char result = c;

			if (reverseMappings.TryGetValue (c, out result)) {
				return result;
			} else {
				return c;
			}
		}

		public string TranslateToDigits(string characters) {
			List<char> digits = new List<char>();

			foreach (char c in characters) 
			{
				char dest = c;

				if (Char.IsDigit (c) == false) {
					if (reverseMappings.TryGetValue (c, out dest)) {
						digits.Add (dest);
					} else {
						if (c == 'z')
							digits.Add ('9');
						else
							digits.Add (c);
					}
				} 
				else 
				{
					digits.Add ('1');
				}
			}

			return new string(digits.ToArray());
		}

	}
}

