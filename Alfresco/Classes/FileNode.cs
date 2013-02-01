using System;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;
using System.IO;
using System.Collections;
using System.Web.Services.Protocols;
using Alfresco.Exceptions;


namespace Alfresco.Classes
{
	public class FileNode : NodeBase
	{
		#region Constructores

		public FileNode()
			: base()
		{ }

		public FileNode(string name, string title, string description)
			: base(name, title, description)
		{ }

		#endregion

		#region Métodos públicos

		///// <summary>
		///// Guarda un fichero en la ruta especificada, si esta no existe la crea
		///// </summary>
		///// <param name="parentPath">Ruta en la que se debe guardar el fichero</param>
		///// <param name="documentPath">Ruta local de la que se debe sacar el fichero, aquí se devuelve la ruta del repositorio donde se ha guardado el  documento</param>
		///// <returns>uuid del documento que se ha salvado</returns>
		//public string CreateFileByParentPath(string parentPath, ref string documentPath)
		//{
		//    string parentId = null;
		//    string documentId = null;

		//    FolderNode nod = new FolderNode();
		//    parentId = nod.CreatePathRecursive(parentPath);
		//    documentId = CreateFileByParentId(parentId, ref documentPath, null);

		//    return documentId;
		//}

		/// <summary>
		/// sube el documento especificado en document a la ruta parentPath, parentPath acaba con el valor de la ruta relativa en el repo del documento
		/// </summary>
		/// <param name="parentPath"></param>
		/// <param name="document"></param>
		/// <returns></returns>
		public string CreateFileByParentPath(ref string parentPath, byte[] document, string documentName)
		{
			string parentId = null;
			string documentId = null;

			FolderNode nod = new FolderNode();
			parentId = nod.CreatePathRecursive(parentPath);
			parentPath = documentName;

			documentId = CreateFileByParentId(parentId, ref parentPath, document);

			return documentId;
		}

		/// <summary>
		/// Guarda un fichero en el nodo especificado
		/// </summary>
		/// <param name="parentId">uuid del nodo donde queremos guardar el documento</param>
		/// <param name="documentPath">Ruta local de la que se debe sacar el fichero, aquí se devuelve la ruta del repositorio donde se ha guardado el  documento</param>
		/// <param name="document">raw document, si este valor es null se intentará leer el documento del documentPath</param>
		/// <returns>uuid del documento que se ha salvado</returns>
		public string CreateFileByParentId(string parentId, ref string documentName, byte[] document)
		{
			if (document == null) return null;

			string documentId = null;
			var mimeType = new MimetypeMap();

			try
			{
				UpdateResult[] updateResult = CreateNode(parentId, null, Constants.TYPE_CONTENT);

				// work around to cast Alfresco.RepositoryWebService.Reference to Alfresco.ContentWebService.Reference 
				RepositoryWebService.Reference rwsRef = updateResult[0].destination;
				ContentWebService.Reference newContentNode = new Alfresco.ContentWebService.Reference();
				newContentNode.path = rwsRef.path;
				newContentNode.uuid = rwsRef.uuid;
				ContentWebService.Store cwsStore = new Alfresco.ContentWebService.Store();
				cwsStore.address = Constants.SPACES_STORE;
				newContentNode.store = cwsStore;

				var contentFormat = new Alfresco.ContentWebService.ContentFormat();
				contentFormat.mimetype = mimeType.GuessMimetype(Name);

				Content lContent = WebServiceFactory.getContentService().write(newContentNode, Constants.PROP_CONTENT, document, contentFormat);
				documentName = ISO9075.Decode(PathUtils.ConvertFromRepositoryPath(lContent.node.path));
				documentId = lContent.node.uuid;
			}
			catch (SoapException ex)
			{
				if (ex.Detail.InnerText.Contains("DuplicateChildNodeNameException"))
				{
					var node = new NodeBase();
					var nodePath = String.Format("{0}/{1}", node.GetPathById(parentId), System.IO.Path.GetFileName(documentName));
					var id = node.GetIdByPath(nodePath);

					throw new DuplicateDocumentException(id, nodePath);
				}
				else
					throw ex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return documentId;
		}

		public string UpdateFileById(string id, byte[] document)
		{
			if (document == null) return string.Empty;

			Id = id;
			RepositoryWebService.Reference reference;
			ResultSetRow row = FindNodeById(id);

			if (row == null)
				throw new NotFoundDocumentException(id, ErrorMessages.DocumentNotFound);

			reference = GetReferenceFromResultSetRow(row);
			Content lContent = UpdateDocument(reference, document);

			return ISO9075.Decode(PathUtils.ConvertFromRepositoryPath(lContent.node.path));
		}

		public string UpdateFileByPath(string path, byte[] document)
		{
			if (document == null) return string.Empty;

			this.Path = path;
			ResultSetRow row = FindNodeByPath(ref path);

			if (row == null)
				throw new NotFoundDocumentException(Id, path, ErrorMessages.DocumentNotFound);

			var reference = GetReferenceFromResultSetRow(row);
			Id = reference.uuid;
			Content lContent = UpdateDocument(reference, document);

			return ISO9075.Decode(PathUtils.ConvertFromRepositoryPath(lContent.node.path));
		}

		///// <summary>
		///// Copia un archivo de una localización a otroa
		///// </summary>
		///// <param name="uuidNodeToCopy"></param>
		///// <param name="DestinationNodePath"></param>
		///// <returns>En DestinationNodePath se devuelve la ruta final del archivo</returns>
		//public string CopyFile(string uuidNodeToCopy, ref string DestinationNodePath)
		//{
		//    if (string.IsNullOrEmpty(uuidNodeToCopy) || string.IsNullOrEmpty(DestinationNodePath))
		//        return null;

		//    string documentId = null;
		//    RepositoryWebService.Store cwsStore = new RepositoryWebService.Store();
		//    cwsStore.address = Constants.SPACES_STORE;

		//    try
		//    {
		//        //Obtenemos la ruta donde se copiara el documento, o la creamos si no existe
		//        FolderNode nod = new FolderNode();
		//        string destNodeId = nod.CreatePathRecursive(DestinationNodePath);

		//        this.Name = GetNodeById(uuidNodeToCopy).Name;
		//        UpdateResult[] rsrDest = CreateNode(destNodeId, null, Constants.TYPE_CONTENT);


		//        ResultSetRow rsrOrigin = FindNodeById(uuidNodeToCopy);
		//        RepositoryWebService.Reference refOrigin = GetReferenceFromResultSetRow(rsrOrigin);


		//        RepositoryWebService.Predicate sourcePredicate = new RepositoryWebService.Predicate(
		//            new Alfresco.RepositoryWebService.Reference[] { refOrigin }, cwsStore, null);


		//        //reference for the target space
		//        RepositoryWebService.ParentReference targetSpace = new RepositoryWebService.ParentReference();
		//        targetSpace.store = spacesStore;
		//        targetSpace.path = DestinationNodePath;
		//        targetSpace.associationType = Constants.ASSOC_CONTAINS;
		//        targetSpace.childName = Name;



		//        //copy content
		//        CMLCopy copy = new CMLCopy();
		//        copy.where = sourcePredicate;
		//        copy.to = targetSpace;

		//        CML cmlCopy = new CML();
		//        cmlCopy.copy = new CMLCopy[] { copy };

		//        //perform a CML update to move the node
		//        UpdateResult[] updateResult = WebServiceFactory.getRepositoryService().update(cmlCopy);
		//        DestinationNodePath = ISO9075.Decode(PathUtils.ConvertFromRepositoryPath(updateResult[0].destination.path));

		//        documentId = updateResult[0].destination.uuid;
		//    }
		//    catch (SoapException ex)
		//    {
		//        if (ex.Detail.InnerText.Contains("DuplicateChildNodeNameException"))
		//        {
		//            var node = new NodeBase();
		//            var nodePath = String.Format("{0}/{1}", DestinationNodePath, this.Name);
		//            var id = node.GetIdByPath(nodePath);

		//            throw new DuplicateDocumentException(id, nodePath);
		//        }
		//        else
		//            throw ex;
		//    }
		//    catch (Exception ex)
		//    {
		//        throw ex;
		//    }

		//    return documentId;
		//}

		//public string CopyFile(string uuidNodeToCopy, ref string DestinationNodePath)
		//{
		//    if (string.IsNullOrEmpty(uuidNodeToCopy) || string.IsNullOrEmpty(DestinationNodePath))
		//        return null;

		//    string documentId = null;

		//    try
		//    {
		//        this.Name = GetNodeById(uuidNodeToCopy).Name;

		//        //Obtenemos la ruta donde se copiara el documento, o la creamos si no existe
		//        FolderNode nod = new FolderNode();
		//        string destNodeId = nod.CreatePathRecursive(DestinationNodePath);


		//        UpdateResult[] updateNode = CreateNode(destNodeId, null, Constants.TYPE_CONTENT);

		//        //reference for the target space
		//        RepositoryWebService.ParentReference targetSpace = new RepositoryWebService.ParentReference();
		//        targetSpace.store = spacesStore;
		//        targetSpace.path = updateNode[0].destination.path;
		//        targetSpace.associationType = Constants.ASSOC_CONTAINS;
		//        targetSpace.childName = Name;



		//        RepositoryWebService.Predicate Source = new RepositoryWebService.Predicate(
		//            new Alfresco.RepositoryWebService.Reference[] { GetReferenceFromResultSetRow(FindNodeById(uuidNodeToCopy)) }, spacesStore, null);


		//        //copy content
		//        CMLCopy copy = new CMLCopy();
		//        copy.where = Source;
		//        copy.to = targetSpace;

		//        CML cmlCopy = new CML();
		//        cmlCopy.copy = new CMLCopy[] { copy };

		//        //perform a CML update to move the node
		//        RepositoryWebService.UpdateResult[] updateResult = WebServiceFactory.getRepositoryService().update(cmlCopy);
		//        DestinationNodePath = ISO9075.Decode(PathUtils.ConvertFromRepositoryPath(updateResult[0].destination.path));

		//        documentId = updateResult[0].destination.uuid;
		//    }
		//    catch (SoapException ex)
		//    {
		//        if (ex.Detail.InnerText.Contains("DuplicateChildNodeNameException"))
		//        {
		//            var node = new NodeBase();
		//            var nodePath = String.Format("{0}/{1}", DestinationNodePath, this.Name);
		//            var id = node.GetIdByPath(nodePath);

		//            throw new DuplicateDocumentException(id, nodePath);
		//        }
		//        else
		//            throw ex;
		//    }
		//    catch (Exception ex)
		//    {
		//        throw ex;
		//    }

		//    return documentId;
		//}

		public string CopyFile(string uuidNodeToCopy, ref string DestinationNodePath)
		{
			if (string.IsNullOrEmpty(uuidNodeToCopy) || string.IsNullOrEmpty(DestinationNodePath))
				return null;

			string documentId = null;

			try
			{
				this.Name = GetNodeById(uuidNodeToCopy).Name;

				//Obtenemos la ruta donde se copiara el documento, o la creamos si no existe
				FolderNode nod = new FolderNode();
				string destNodeId = nod.CreatePathRecursive(DestinationNodePath);

				RepositoryWebService.Predicate Source = new RepositoryWebService.Predicate(
					new Alfresco.RepositoryWebService.Reference[] { GetReferenceFromResultSetRow(FindNodeById(uuidNodeToCopy)) },
					spacesStore,
					null);

				//copy content
				CMLCopy copy = new CMLCopy();
				copy.where = Source;
				copy.to = createParentReference(destNodeId, Constants.ASSOC_CONTAINS, Name);

				CML cmlCopy = new CML();
				cmlCopy.copy = new CMLCopy[] { copy };

				//perform a CML update to move the node
				RepositoryWebService.UpdateResult[] updateResult = WebServiceFactory.getRepositoryService().update(cmlCopy);
				DestinationNodePath = ISO9075.Decode(PathUtils.ConvertFromRepositoryPath(updateResult[0].destination.path));

				documentId = updateResult[0].destination.uuid;
			}
			catch (SoapException ex)
			{
				if (ex.Detail.InnerText.Contains("DuplicateChildNodeNameException"))
				{
					var node = new NodeBase();
					var nodePath = String.Format("{0}/{1}", DestinationNodePath, this.Name);
					var id = node.GetIdByPath(nodePath);

					throw new DuplicateDocumentException(id, nodePath);
				}
				else
					throw ex;
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return documentId;
		}

		public void SetParameters(Hashtable parameters)
		{

		}

		#endregion

		#region Métodos privados

		private Content UpdateDocument(Alfresco.RepositoryWebService.Reference reference, byte[] document)
		{
			MimetypeMap mimeType = new MimetypeMap();

			var newContentNode = new Alfresco.ContentWebService.Reference();
			newContentNode.path = reference.path;
			newContentNode.uuid = reference.uuid;
			Alfresco.ContentWebService.Store cwsStore = new Alfresco.ContentWebService.Store();
			cwsStore.address = Constants.SPACES_STORE;
			newContentNode.store = cwsStore;

			var contentFormat = new Alfresco.ContentWebService.ContentFormat();
			contentFormat.mimetype = mimeType.GuessMimetype(Name);

			return WebServiceFactory.getContentService().write(newContentNode, Constants.PROP_CONTENT, document, contentFormat);
		}

		#endregion

	}
}
