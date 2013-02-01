using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Alfresco.Classes
{
	public class PathUtils
	{
		public static string RootPath = string.Empty;

		/// <summary>
		/// Las rutas llegan en formato /CF0001/xxx y se devuelven en formato //cm:CF0001//cm:xxx
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string ConvertToRepositoryPath(string path)
		{
			StringBuilder lResult = new StringBuilder();
			path = NormalicePath(path);
			path = path.Substring(1, path.Length - 1);

			string[] parts = path.Split('/');
			foreach (string part in parts)
			{
				if (part.StartsWith("app:") || part.StartsWith("cm:"))
				{
					lResult.AppendFormat("//{0}", part);
				}
				else
				{
					lResult.AppendFormat("//cm:{0}", part);
				}
			}

			return lResult.ToString();
		}

		/// <summary>
		/// Las rutas llegan en formato //cm:CF0001//cm:xxxy o //app:company_home//cm:CF0001//cm:xxx se devuelven en formato /CF0001/xxx 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string ConvertFromRepositoryPath(string path)
		{
			path = Regex.Replace(path, RootPath, string.Empty);
			path = Regex.Replace(path, @"/cm:", "/");
			if (!path.StartsWith("/"))
				path = string.Format("/{0}", path);
			return path;
		}

		public static string CovertFromModelPathToRepositoryPath(string path)
		{
			path = path.Replace("{http://www.alfresco.org/model/application/1.0}", "app:");
			path = path.Replace("{http://www.alfresco.org/model/site/1.0}", "st:");
			path = path.Replace("{http://www.alfresco.org/model/content/1.0}", "cm:");
			path = path.Replace(Constants.PROP_CONTENT, "/cm:");

			return path;
		}


		/// <summary>
		/// Añade un / al principio si no se tiene y elimina el del final si estuviera presente
		/// en caso de que se encuentre Backslash dobles o simples tambien los combierte a slash
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string NormalicePath(string path)
		{
			if (path.StartsWith("//cm:")) return path;

			path = path.Replace("\\", "/");
			path = path.Replace("//", "/");
			if (!path.StartsWith("/"))
				path = string.Format("/{0}", path);

			if (path.LastIndexOf("/") == path.Length - 1)
				path = path.Substring(0, path.Length - 1);

			return path;
		}
	}
}
