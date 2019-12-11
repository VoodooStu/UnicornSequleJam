namespace VoodooPackages.Tech.Items
{
	public static class ExtensionMethodsDouble
	{
		private static string[] StringUnits =
		{
			"", "k", "m", "b", "t", "q", "Q", "u", "U",
			"s", "S", "p", "P", "o", "O", "n", "N", "d", "D",
			"g", "G", "h", "H", "l", "L", "i", "I", "j", "J", "n", "N",
			"c", "C", "x", "X", "w", "W", "y", "Y", "z", "Z"
		};

		private static string GetNewIndex(int index)
		{
			char first = (char)('a' + index / 26);
			char second = (char)('a' + index % 26);

			return first.ToString() + second.ToString();
		}

		public static string ToShortString(this double value)
		{
			if (value < 1)
				return "0";

			string str = value.ToString("#");
			if (str == "Infinity")
				return "Inf";

			int numDigits = str.Length;
			if (str.Length > 3)
			{
				string result = str.Substring(0, 3);
				int nb = numDigits % 3;
				if (nb > 0)
				{
					result = result.Insert(nb, ".");
				}
				int index = (numDigits - 1) / 3;

				string unit;
				if (index < StringUnits.Length)
				{
					unit = StringUnits[index];
				}
				else
				{
					unit = GetNewIndex(index - StringUnits.Length);
				}
				return result + unit;
			}

			return str;
		}
	}
}