using System;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;
using System.Text.RegularExpressions;
using System.Text;


namespace Alfresco.Classes
{
	public class FolderNode : NodeBase
	{

		#region Constructores

		public FolderNode()
			: base()
		{ }

		public FolderNode(string name, string title, string description)
			: base(name, title, description)
		{ }

		#endregion


		#region Métodos públicos

		public string CreateFolderByParentPath(string parentPath)
		{
			UpdateResult[] updateResult = CreateNode(null, parentPath, Constants.TYPE_FOLDER);

			return updateResult[0].destination.uuid;
		}

		public string CreateFolderByParentId(string parentId)
		{
			UpdateResult[] updateResult = CreateNode(parentId, null, Constants.TYPE_FOLDER);

			return updateResult[0].destination.uuid;
		}

		/// <summary>
		/// Si la ruta a crear existe, nos devuelve su uid, si no existe la crea y nos devuelve su uid
		/// </summary>
		/// <param name="parentPath">Ruta a crear</param>
		/// <returns>uid de la ruta creada</returns>
		public string CreatePathRecursive(string parentPath)
		{
			string lResult = null;
			var tmpPath = new StringBuilder();

			Content c = GetContentByPath(parentPath);


			if (c != null)
			{
				lResult = c.node.uuid;
			}
			else
			{
				string[] parts = Regex.Split(parentPath, @"//cm:");
				for (int i = 0; i < parts.Length - 1; i++)
				{
					if (parts[i].StartsWith("//app:"))
						tmpPath.Append(parts[i]);
					else
						tmpPath.AppendFormat("//cm:{0}", parts[i]);
				}

				lResult = CreatePathRecursive(tmpPath.ToString());
				Path = parentPath;
				Name = parts[parts.Length - 1];
				lResult = CreateFolderByParentId(lResult);
			}

			return lResult;


			//ResultSetRow row = null;

			//row = FindNodeByPath(ref parentPath);

			//if (row != null)
			//{
			//    lResult = row.node.id;
			//}
			//else
			//{
			//    string[] parts = Regex.Split(parentPath, @"/");
			//    for (int i = 0; i < parts.Length - 1; i++)
			//    {
			//        tmpPath += "/" + parts[i];
			//    }

			//    lResult = CreatePathRecursive(tmpPath);
			//    path = parentPath;
			//    name = parts[parts.Length - 1];
			//    lResult = CreateFolderByParentId(lResult);
			//}

			//return lResult;
		}

		#endregion

	}
}
