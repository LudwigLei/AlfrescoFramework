using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alfresco;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;
using Alfresco.AuthoringWebService;

namespace Samples
{
    public partial class Browse : Form
    {
        private Alfresco.RepositoryWebService.Store spacesStore;

        private RepositoryService repoService;
        private ContentService contentService;
        private AuthoringService authoringService;


        private Alfresco.RepositoryWebService.Reference currentReference;
        private ArrayList parentReferences = new ArrayList();

        /// <summary>
        /// Default constructor
        /// </summary>
        public Browse()
        {
            InitializeComponent();

            // Initialise the reference to the spaces store
            this.spacesStore = new Alfresco.RepositoryWebService.Store();
            this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
            this.spacesStore.address = "SpacesStore";
        }

        /// <summary>
        /// The form load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browse_Load(object sender, EventArgs e)
        {
            // Ensure the user has been authenticated
            if (AuthenticationUtils.IsSessionValid == false)
                AuthenticationUtils.startSession("admin", "sametsis");

            // Get a repository and content service from the web service factory
            this.repoService = WebServiceFactory.getRepositoryService();
            this.contentService = WebServiceFactory.getContentService();
            this.authoringService = WebServiceFactory.getAuthoringService();

            // Populate the list box 
            populateListBox();
        }

        /// <summary>
        /// Populate the list box with the children of the company home
        /// </summary>
        private void populateListBox()
        {
            Alfresco.RepositoryWebService.Reference reference = new Alfresco.RepositoryWebService.Reference();
            reference.store = this.spacesStore;
            reference.path = "/app:company_home";

            populateListBox(reference);
        }

        /// <summary>
        /// Populate the list with the children of the passed reference
        /// </summary>
        /// <param name="reference"></param>
        private void populateListBox(Alfresco.RepositoryWebService.Reference reference)
        {
            // Clear the list
            listViewBrowse.Clear();

            // Set the current reference
            this.currentReference = reference;

            // Query for the children of the reference
            QueryResult result = this.repoService.queryChildren(reference);

            if (result.resultSet.rows != null)
            {
                int index = 0;
                foreach (ResultSetRow row in result.resultSet.rows)
                {
                    // Get the name of the node
                    string name = null;
                    foreach (Alfresco.RepositoryWebService.NamedValue namedValue in row.columns)
                    {
                        if (namedValue.name.Contains("name") == true)
                        {
                            name = namedValue.value;
                        }
                    }

                    // Create the list view item that will correspond to the child node
                    ListViewItem item = new ListViewItem();
                    item.Text = name;
                    if (row.node.type.Contains("folder") == true)
                    {
                        item.ImageIndex = 0;
                    }
                    else
                    {
                        item.ImageIndex = 1;
                    }
                    item.Tag = row.node;

                    // Add the item to the list
                    if (row.node.type.Contains("folder") == true)
                    {
                        listViewBrowse.Items.Insert(index, item);
                        index++;
                    }
                    else
                    {
                        listViewBrowse.Items.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Double click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewBrowse_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = listViewBrowse.SelectedItems[0];
            if (item != null)
            {
                ResultSetRowNode node = item.Tag as ResultSetRowNode;
                if (node != null)
                {
                    if (node.type.Contains("folder") == true)
                    {
                        // Create the reference for the node selected
                        Alfresco.RepositoryWebService.Reference reference = new Alfresco.RepositoryWebService.Reference();
                        reference.store = this.spacesStore;
                        reference.uuid = node.id;

                        // Parent references
                        this.parentReferences.Add(this.currentReference);

                        // Populate the list with the children of the selected node
                        populateListBox(reference);
                    }
                    else
                    {
                        // Create the reference for the node selected
                        Alfresco.ContentWebService.Store spacesStore2 = new Alfresco.ContentWebService.Store();
                        spacesStore2.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
                        spacesStore2.address = "SpacesStore";

                        Alfresco.ContentWebService.Reference reference = new Alfresco.ContentWebService.Reference();
                        reference.store = spacesStore2;
                        reference.uuid = node.id;

                        // Lets try and get the content
                        Alfresco.ContentWebService.Predicate predicate = new Alfresco.ContentWebService.Predicate();
                        predicate.Items = new Object[] { reference };
                        Content[] contents = this.contentService.read(predicate, "{http://www.alfresco.org/model/content/1.0}content");
                        Content content = contents[0];
                        if (content.url != null && content.url.Length != 0)
                        {
                            string url = content.url + "?ticket=" + AuthenticationUtils.Ticket;
                            webBrowser.Url = new Uri(url);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Click handler for the 'Go Up' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.parentReferences.Count != 0)
            {
                Alfresco.RepositoryWebService.Reference reference = this.parentReferences[this.parentReferences.Count - 1] as Alfresco.RepositoryWebService.Reference;
                this.parentReferences.RemoveAt(this.parentReferences.Count - 1);
                populateListBox(reference);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem item = listViewBrowse.SelectedItems[0];
            if (item != null)
            {
                ResultSetRowNode node = item.Tag as ResultSetRowNode;
                if (node != null)
                {
                    if (node.type.Contains("folder") == false)
                    {
                        // Create the reference for the node selected
                        Alfresco.AuthoringWebService.Store spacesStore2 = new Alfresco.AuthoringWebService.Store();
                        spacesStore2.scheme = Alfresco.AuthoringWebService.StoreEnum.workspace;
                        spacesStore2.address = "SpacesStore";

                        Alfresco.AuthoringWebService.Reference reference = new Alfresco.AuthoringWebService.Reference();
                        reference.store = spacesStore2;
                        reference.uuid = node.id;

                        // Lets try to check out
                        Alfresco.AuthoringWebService.Predicate predicate = new Alfresco.AuthoringWebService.Predicate();
                        predicate.Items = new Object[] { reference };
                        Alfresco.AuthoringWebService.ParentReference pr = new Alfresco.AuthoringWebService.ParentReference();
                        pr.store = spacesStore2; ;
                        pr.uuid = this.currentReference.uuid;
                        pr.associationType = Constants.ASSOC_CONTAINS;
                        pr.childName = Constants.createQNameString(Constants.NAMESPACE_CONTENT_MODEL, item.Text);
                        this.authoringService.checkout(predicate, pr);
                        int i = 0;

                    }
                    else
                    {

                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Alfresco.RepositoryWebService.Store[] stores = this.repoService.getStores();

            Alfresco.RepositoryWebService.Store vStore = stores[3];

            ListViewItem item = listViewBrowse.SelectedItems[0];
            if (item != null)
            {
                ResultSetRowNode node = item.Tag as ResultSetRowNode;
                if (node != null)
                {
                    if (node.type.Contains("folder") == false)
                    {
                        // Create the reference for the node selected
                        Alfresco.AuthoringWebService.Store spacesStore2 = new Alfresco.AuthoringWebService.Store();
                        spacesStore2.scheme = Alfresco.AuthoringWebService.StoreEnum.workspace;
                        spacesStore2.address = "SpacesStore";

                        Alfresco.AuthoringWebService.Reference reference = new Alfresco.AuthoringWebService.Reference();
                        reference.store = spacesStore2;
                        reference.uuid = node.id;

                        VersionHistory VH = this.authoringService.getVersionHistory(reference);

                        int i = 0;
                        char[] temp = new char[1];
                        temp[0] = '0';
                        string versions = new string(temp);
                        Alfresco.AuthoringWebService.Version first;
                        foreach (Alfresco.AuthoringWebService.Version version in VH.versions)
                        {
                            if (i == 0)
                                first = version;
                            versions += version.label + (";") + version.id.uuid + (";");
                        }

                        {
                            // Create the reference for the node selected
                            Alfresco.ContentWebService.Store spacesStore3 = new Alfresco.ContentWebService.Store();
                            spacesStore3.scheme = Alfresco.ContentWebService.StoreEnum.versionStore;
                            spacesStore3.address = vStore.address;

                            Alfresco.ContentWebService.Reference reference1 = new Alfresco.ContentWebService.Reference();
                            reference1.store = spacesStore3;
                            reference1.uuid = VH.versions[VH.versions.GetUpperBound(0)].id.uuid;

                            // Lets try and get the content
                            Alfresco.ContentWebService.Predicate predicate = new Alfresco.ContentWebService.Predicate();
                            predicate.Items = new Object[] { reference1 };
                            Content[] contents = this.contentService.read(predicate, "{http://www.alfresco.org/model/content/1.0}content");
                            Content content = contents[0];
                            if (content.url != null && content.url.Length != 0)
                            {
                                string url = content.url + "?ticket=" + AuthenticationUtils.Ticket;
                                webBrowser.Url = new Uri(url);
                            }


                        }

                    }
                    else
                    {

                    }
                }
            }


        }
    }
}