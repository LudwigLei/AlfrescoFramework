using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alfresco;

namespace Samples
{
	class REMIX
	{
		//public void asignarPermisos(string espacioID, NewUserDetails user)
		//{
		//    Alfresco.AccessControlWebService.Reference Ref = new Alfresco.AccessControlWebService.Reference();
		//    Alfresco.AccessControlWebService.Store spacesS = new Alfresco.AccessControlWebService.Store();

		//    spacesS.scheme = Alfresco.AccessControlWebService.StoreEnum.workspace;
		//    spacesS.address = "SpacesStore;"
		//    Ref.store = spacesS;
		//    Ref.uuid = espacioID;
		//    Ref.path = null;

		//    Alfresco.AccessControlWebService.Predicate pred = new Alfresco.AccessControlWebService.Predicate();
		//    pred.Items = new Object[] { Ref, spacesS, null };

		//    Alfresco.AccessControlWebService.ACE[] aceRemove = new Alfresco.AccessControlWebService.ACE[1];
		//    aceRemove[0] = new Alfresco.AccessControlWebService.ACE();
		//    aceRemove[0].authority = user.userName;
		//    aceRemove[0].permission = Constants.ALL_PERMISSIONS;//Constants.COORDINATOR;
		//    aceRemove[0].accessStatus = Alfresco.AccessControlWebService.AccessStatus.declined;

		//    WebServiceFactory.getAccessControlService().removeACEs(pred, aceRemove);

		//    Alfresco.AccessControlWebService.ACE[] aceWrite = new Alfresco.AccessControlWebService.ACE[1];
		//    aceWrite[0] = new Alfresco.AccessControlWebService.ACE();
		//    aceWrite[0].authority = user.userName;
		//    aceWrite[0].permission = "Consumer";//Consumer Permissions
		//    aceWrite[0].accessStatus = Alfresco.AccessControlWebService.AccessStatus.acepted;

		//    WebServiceFactory.getAccessControlService().addACEs(pred, aceWrite);
		//}

		public string getHomeFolder(string usuario)
		{
			Alfresco.AdministrationWebService.AdministrationService administrationService = new Alfresco.AdministrationWebService.AdministrationService();
			administrationService = WebServiceFactory.getAdministrationService();
			Alfresco.AdministrationWebService.UserDetails detallesUser = new Alfresco.AdministrationWebService.UserDetails();
			detallesUser = administrationService.getUser(usuario);
			string homeFolder = string.Empty;
			for (int i = 0; i < detallesUser.properties.Length; i++)
			{
				if (detallesUser.properties[i].name.ToString() == "{http://www.alfresco.org/model/content/1.0}homeFolder")
				{
					homeFolder = detallesUser.properties[i].value.ToString();
					i = detallesUser.properties.Length;
				}
			}
			return homeFolder;
		}

		//public QueryResult Buscar(RepositoryService repo)
		//{
		//    try
		//    {

		//        string query = "TYPE:\"{http://www.alfresco.org/model/content/1.0}person\"";

		//        // Create a query object
		//        Query query = new Query();
		//        query.language = QueryLanguageEnum.lucene;
		//        query.statement = query;

		//        QueryResult result = repo.query(repo.spacesStore, query, false);

		//        return result;

		//    }
		//    catch (Exception ex)
		//    {
		//        return null;
		//    }
		//}

		//        public string createUser(string UserName, string HomeFolder, string ticket)
		//        {
		//            try 
		//            {
		//                //Para crearusuarios debes estar logueado como administrador
		//Login login = new Login();
		//                AuthenticationUtils.startSession(login.Conectar("user","password");

		//                NewUserDetails[] users = new NewUserDetails[1];
		//                users[0] = new NewUserDetails();
		//                Random rnd = new Random();
		//                users[0].password = rnd.Next(10000, 99999).ToString();
		//                users[0].userName = UserName;           

		//                Alfresco.AdministrationWebService.NamedValue[] properties = new Alfresco.AdministrationWebService.NamedValue[5];

		//                Alfresco.AdministrationWebService.NamedValue nameProperty = new Alfresco.AdministrationWebService.NamedValue();

		//                nameProperty.name = Constants.PROP_USER_FIRSTNAME;
		//                nameProperty.value = UserName;
		//                nameProperty.isMultiValue = false;
		//                properties[0] = nameProperty;

		//                // Create the properties list
		//                nameProperty = new Alfresco.AdministrationWebService.NamedValue();
		//                nameProperty.name = Constants.PROP_USER_LASTNAME;
		//                nameProperty.value = "User"; 
		//                nameProperty.isMultiValue = false;
		//                properties[1] = nameProperty;

		//                // User email
		//                nameProperty = new Alfresco.AdministrationWebService.NamedValue();
		//                nameProperty.name = Constants.PROP_USER_EMAIL;
		//                nameProperty.value = UserName;
		//                nameProperty.isMultiValue = false;
		//                properties[2] = nameProperty;

		//                // Home folder
		//                nameProperty = new Alfresco.AdministrationWebService.NamedValue();
		//                nameProperty.name = Constants.PROP_USER_HOMEFOLDER;
		//                nameProperty.value = HomeFolder;
		//                nameProperty.isMultiValue = false;
		//                properties[3] = nameProperty;

		//                // Organization ID
		//                nameProperty = new Alfresco.AdministrationWebService.NamedValue();
		//                nameProperty.name = Constants.PROP_USER_ORGID;
		//                nameProperty.value = "";
		//                nameProperty.isMultiValue = false;
		//                properties[4] = nameProperty;

		//                users[0].properties = properties;

		//                WebServiceFactory.getAdministrationService().createUsers(users);
		//                asignarPermisos(HomeFolder, users[0]);
		//                //createPersonProperties
		//                Alfresco.AuthenticationUtils.setTicket(ticket);

		//                return users[0].password;

		//            }
		//            catch (Exception ex)
		//            {
		//                return ex.Message.ToString();
		//            }
		//              }
	}
}
