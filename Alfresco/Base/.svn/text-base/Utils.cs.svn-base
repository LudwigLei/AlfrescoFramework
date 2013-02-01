using System;
using Alfresco.RepositoryWebService;

namespace Alfresco
{
	public static class Utils
	{
		public static NamedValue createNamedValue(String name, String value)
		{
			NamedValue namedValue = new NamedValue();
			namedValue.name = name;
			namedValue.value = value;
			return namedValue;
		}

		public static string GetPropertyValue(NamedValue[] namedValues, string propertyName)
		{
			string lResult = null;
			foreach (NamedValue nValue in namedValues)
			{
				if (nValue.name.Contains(propertyName))
				{
					lResult = nValue.value;
				}
			}
			return lResult;
		}
	}
}
