using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alfresco.ContentWebService;
using Alfresco.RepositoryWebService;
using System.Diagnostics;
using System.Web.Services.Protocols;
using Alfresco.Exceptions;


namespace Alfresco.Classes
{
	public class NodeBase
	{
		#region Declaraciones

		private String name;
		private String title;
		private String description;
		private String id;
		private String url;
		private String path;
		protected NamedValue[] properties;

		protected RepositoryWebService.Store spacesStore = new RepositoryWebService.Store(RepositoryWebService.StoreEnum.workspace, Constants.SPACES_STORE);

		#endregion

		#region Propiedades

		public String Name
		{
			get
			{
				if (name == null && properties != null)
				{
					name = Utils.GetPropertyValue(properties, "name");
				}
				return name;
			}
			set { this.name = value; }
		}

		public String Title
		{
			get
			{
				if (title == null && properties != null)
				{
					title = Utils.GetPropertyValue(properties, "title");
				}
				return title;
			}
			set { this.title = value; }
		}

		public String Description
		{
			get
			{
				if (description == null && properties != null)
				{
					description = Utils.GetPropertyValue(properties, "description ");
				}
				return description;
			}
			set { this.description = value; }
		}

		public String Id
		{
			get { return id; }
			set { id = value; }
		}

		public String Path
		{
			get
			{
				if (path == null && properties != null)
				{
					path = Utils.GetPropertyValue(properties, "path");
				}
				return path;
			}
			set { path = value; }
		}

		public String Url
		{
			get
			{
				if (url == null)
				{
					url = FolderNode.GetContentById(id).url;
				}
				return url;
			}
			set { url = value; }
		}

		#endregion

		#region Métodos estáticos

		//[DebuggerStepThrough()]
		public static Content GetContentById(string documentId)
		{
			Content lResult = null;

			try
			{
				Alfresco.ContentWebService.Store spacesStore = new Alfresco.ContentWebService.Store();
				spacesStore.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
				spacesStore.address = Constants.SPACES_STORE;

				var reference = new ContentWebService.Reference();
				reference.store = spacesStore;
				reference.uuid = documentId;

				var predicate = new ContentWebService.Predicate();
				predicate.Items = new Object[] { reference };
				Content[] contents = WebServiceFactory.getContentService().read(predicate, "{http://www.alfresco.org/model/content/1.0}content");

				if (contents.Length != 0)
				{
					lResult = contents[0];
				}
			}
			catch (SoapException ex)
			{
				if (ex.Detail.InnerText.Contains("Node does not exist"))
					throw new NotFoundDocumentException(documentId, ErrorMessages.NoData, ErrorMessages.DocumentNotFound);
				else
					throw ex;
			}

			return lResult;
		}

		//[DebuggerStepThrough()]
		public static Content GetContentByPath(string documentPath)
		{
			Content lResult = null;

			try
			{
				var spacesStore = new ContentWebService.Store();
				spacesStore.scheme = ContentWebService.StoreEnum.workspace;
				spacesStore.address = Constants.SPACES_STORE;

				var reference = new ContentWebService.Reference();
				reference.store = spacesStore;
				reference.path = PathUtils.ConvertToRepositoryPath(documentPath);

				var predicate = new ContentWebService.Predicate();
				predicate.Items = new Object[] { reference };
				Content[] contents = WebServiceFactory.getContentService().read(predicate, "{http://www.alfresco.org/model/content/1.0}content");

				if (contents.Length != 0)
				{
					lResult = contents[0];
				}
			}
			catch (SoapException ex)
			{
				if (ex.Detail.InnerText.Contains("Node does not exist"))
					throw new NotFoundDocumentException(ErrorMessages.NoData, documentPath, ErrorMessages.DocumentNotFound);
				//else
				//    throw ex;
			}

			return lResult;
		}


		public static NodeBase GetNodeById(string documentId)
		{
			NodeBase lNode = null;

			Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
			spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
			spacesStore.address = Constants.SPACES_STORE;


			// Create a query object
			Alfresco.RepositoryWebService.Query query = new Alfresco.RepositoryWebService.Query();
			query.language = Constants.QUERY_LANG_LUCENE;
			query.statement = string.Format("@sys\\:node-uuid:\"{0}\"", documentId);
			QueryResult result = WebServiceFactory.getRepositoryService().query(spacesStore, query, true);

			if (result.resultSet.rows != null)
			{
				ResultSetRow row = result.resultSet.rows[0];
				lNode = new NodeBase();
				lNode.properties = new NamedValue[row.columns.Length];
				row.columns.CopyTo(lNode.properties, 0);
				lNode.id = documentId;
			}

			return lNode;
		}

		#endregion

		#region Constructores

		public NodeBase()
		{
		}

		public NodeBase(string name, string title, string description)
		{
			this.name = name;
			this.title = title;
			this.description = description;
		}

		public NodeBase(string name)
		{
			this.name = name;
		}

		#endregion

		#region Métodos protected

		protected UpdateResult[] CreateNode(string parentId, string parentPath, string nodeType)
		{
			UpdateResult[] result = null;
			var parent = new RepositoryWebService.ParentReference(
				spacesStore,
				parentId,
				parentPath,
				Constants.ASSOC_CONTAINS,
				Constants.createQNameString(Constants.NAMESPACE_CONTENT_MODEL, name)
				);

			//build properties
			BuildCustomProperties();

			//create operation
			CMLCreate create = new CMLCreate();
			create.id = "1";
			create.parent = parent;
			create.type = nodeType;// Constants.TYPE_CONTENT;
			create.property = properties;

			//build the CML object
			CML cml = new CML();
			cml.create = new CMLCreate[] { create };

			//perform a CML update to create the node
			result = WebServiceFactory.getRepositoryService().update(cml);

			return result;
		}

		protected virtual void BuildCustomProperties()
		{
			properties = new NamedValue[3];
			properties[0] = Utils.createNamedValue(Constants.PROP_NAME, name);
			properties[1] = Utils.createNamedValue(Constants.PROP_TITLE, title);
			properties[2] = Utils.createNamedValue(Constants.PROP_DESCRIPTION, description);
		}

		protected RepositoryWebService.ParentReference createParentReference(string uuidNode, string assocType, string assocName)
		{
			RepositoryWebService.ParentReference parentReference = new RepositoryWebService.ParentReference();
			parentReference.associationType = assocType;
			parentReference.childName = assocName;
			parentReference.store = new RepositoryWebService.Store(RepositoryWebService.StoreEnum.workspace, Constants.SPACES_STORE);
			parentReference.uuid = uuidNode;

			return parentReference;
		}

		#endregion

		#region Métodos públicos

		public ResultSetRow FindNodeById(string nodeId)
		{
			ResultSetRow lResult = null;

			Alfresco.RepositoryWebService.Query query = new Alfresco.RepositoryWebService.Query();
			query.language = Constants.QUERY_LANG_LUCENE;
			//TODO: Consultas que sí funcionan:
			//query.statement = "PATH:\"//app:company_home//cm:Pruebas//cm:Test_Folder/*\""; 
			//query.statement = "PATH:\"//cm:Pruebas//cm:Test_Folder/*\"";  Todos los archivos de un folder
			//query.statement = "PATH:\"//cm:Pruebas//cm:Test_Folder\"";   Devuelve un folder
			query.statement = "@sys\\:node-uuid:\"" + nodeId + "\"";

			QueryResult result = WebServiceFactory.getRepositoryService().query(spacesStore, query, false);

			if (result.resultSet.rows != null)
			{
				lResult = result.resultSet.rows[0];
			}

			return lResult;
		}

		public ResultSetRow FindNodeByPath(ref string nodePath)
		{
			ResultSetRow lResult = null;
			nodePath = PathUtils.NormalicePath(nodePath);

			string queryString = PathUtils.ConvertToRepositoryPath(System.IO.Path.GetDirectoryName(nodePath) + "/" + ISO9075.Encode(System.IO.Path.GetFileName(nodePath)));
			var query = new RepositoryWebService.Query();
			query.language = Constants.QUERY_LANG_LUCENE;
			//TODO: Consultas que sí funcionan:
			//query.statement = "PATH:\"//app:company_home//cm:Pruebas//cm:Test_Folder/*\""; 
			//query.statement = "PATH:\"//cm:Pruebas//cm:Test_Folder/*\"";  Todos los archivos de un folder
			//query.statement = "PATH:\"//cm:Pruebas//cm:Test_Folder\"";   Devuelve un folder
			query.statement = "PATH:\"" + queryString + "\"";

			QueryResult result = WebServiceFactory.getRepositoryService().query(spacesStore, query, false);

			if (result.resultSet.rows != null)
			{
				lResult = result.resultSet.rows[0];
			}

			return lResult;
		}

		public string GetIdByPath(string nodePath)
		{
			ResultSetRow row = FindNodeByPath(ref nodePath);
			return row.node.id;
		}

		public string GetPathById(string id)
		{
			ResultSetRow row = FindNodeById(id);
			return PathUtils.ConvertFromRepositoryPath(PathUtils.CovertFromModelPathToRepositoryPath(row.columns[0].value));
		}

		public RepositoryWebService.Reference GetReferenceFromResultSetRow(ResultSetRow row)
		{
			RepositoryWebService.Reference reference = null;

			reference = new Alfresco.RepositoryWebService.Reference();
			reference.uuid = row.node.id;
			reference.store = spacesStore;

			NamedValue[] namedValues = row.columns;

			Dictionary<string, string> namedValuesMap = new Dictionary<string, string>();
			// iterate through the columns of the result set to extract specific named values

			foreach (NamedValue namedValue in namedValues)
			{
				namedValuesMap.Add(namedValue.name, namedValue.value);
			}
			String path = namedValuesMap["{http://www.alfresco.org/model/content/1.0}path"];
			name = namedValuesMap["{http://www.alfresco.org/model/content/1.0}name"];

			reference.path = PathUtils.CovertFromModelPathToRepositoryPath(path);

			return reference;
		}

		public void DeleteNodeById(string id)
		{
			var reference = new RepositoryWebService.Reference();
			reference.store = spacesStore; //la definicion del store la explico mas abajo
			reference.uuid = id;
			DeleteNodes(new Alfresco.RepositoryWebService.Reference[] { reference });
		}

		public void DeleteNodeByPath(string path)
		{
			var reference = new RepositoryWebService.Reference();
			reference.store = spacesStore; //la definicion del store la explico mas abajo
			reference.path = path;
			DeleteNodes(new RepositoryWebService.Reference[] { reference });
		}

		#endregion

		#region Métodos internal / private

		internal void AddProperty(string propertyName, string propertyValue)
		{
			bool propertyExists = false;

			if (properties == null)
			{
				properties = new NamedValue[1];
				properties[0] = Utils.createNamedValue(propertyName, propertyValue);
			}
			else
			{
				foreach (NamedValue nv in properties)
				{
					if (nv.name.Equals(propertyName))
					{
						nv.value = propertyValue;
						propertyExists = true;
						break;
					}
				}

				if (!propertyExists)
				{
					var tmp = new NamedValue[properties.Length + 1];
					properties.CopyTo(tmp, 0);
					properties = tmp;
				}
			}
		}

		private void DeleteNodes(Alfresco.RepositoryWebService.Reference[] references)
		{
			var predicate = new RepositoryWebService.Predicate(references, spacesStore, null);

			//delete content
			CMLDelete delete = new CMLDelete();
			delete.where = predicate;

			CML cmlRemove = new CML();
			cmlRemove.delete = new CMLDelete[] { delete };

			//perform a CML update to remove the node
			WebServiceFactory.getRepositoryService().update(cmlRemove);
		}

		#endregion
	}
}
