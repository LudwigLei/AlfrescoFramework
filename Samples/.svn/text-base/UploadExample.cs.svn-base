using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Alfresco;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;

namespace Samples
{
	public partial class UploadExample : Form
	{

		private RepositoryService repoService;
		private TreeViewBrowse browse = null;
		private MimetypeMap mimeType = new MimetypeMap();

		// set by the TreeView form
		private String locationUuid;

		// sets the Location field on this form
		public String LocationName
		{
			set
			{
				this.tbLocation.Text = value;
			}
		}

		// used to to store the uuid of the folder
		public String LocationUuid
		{
			set { locationUuid = value; }
		}



		public UploadExample()
		{
			InitializeComponent();

			AuthenticationUtils.startSession("admin", "sametsis");

			// Get the repository service
			this.repoService = WebServiceFactory.getRepositoryService();
		}

		// Opens a file select dialoge box
		private void btnSelect_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.InitialDirectory = "c:\\";
			//openFileDialog1.Filter = "jpg files (*.jpg)|*.jpg";
			openFileDialog1.FilterIndex = 2;
			openFileDialog1.RestoreDirectory = true;

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{

				String file = openFileDialog1.FileName;
				this.textBox1.Text = file;
			}

		}

		private void btnUpload_Click(object sender, EventArgs e)
		{
			try
			{
				String file = this.textBox1.Text;

				if (file.Equals(""))
				{
					MessageBox.Show("Please select a file");
					this.btnSelect.Focus();
					return;
				}

				if (this.tbLocation.Text.Equals(""))
				{
					MessageBox.Show("Please select the location");
					this.btnLocation.Focus();
					return;
				}

				int start = file.LastIndexOf("\\") + 1;
				int length = file.Length - start;
				// get the filename only
				String fileName = file.Substring(start, length);

				if (file == null || file.Equals(""))
				{
					MessageBox.Show("please select a file");
					return;
				}
				// Display a wait cursor while the file is uploaded
				Cursor.Current = Cursors.WaitCursor;

				// Initialise the reference to the spaces store
				Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
				spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
				spacesStore.address = "SpacesStore";

				// Create the parent reference, the company home folder
				Alfresco.RepositoryWebService.ParentReference parentReference = new Alfresco.RepositoryWebService.ParentReference();
				parentReference.store = spacesStore;
				//                parentReference.path = "/app:company_home";
				parentReference.uuid = this.locationUuid;


				parentReference.associationType = Constants.ASSOC_CONTAINS;
				parentReference.childName = Constants.createQNameString(Constants.NAMESPACE_CONTENT_MODEL, fileName);

				// Create the properties list
				NamedValue nameProperty = new NamedValue();
				nameProperty.name = Constants.PROP_NAME;
				nameProperty.value = fileName;
				nameProperty.isMultiValue = false;

				NamedValue[] properties = new NamedValue[2];
				properties[0] = nameProperty;
				nameProperty = new NamedValue();
				nameProperty.name = Constants.PROP_TITLE;
				nameProperty.value = fileName;
				nameProperty.isMultiValue = false;
				properties[1] = nameProperty;

				// Create the CML create object
				CMLCreate create = new CMLCreate();
				create.parent = parentReference;
				create.id = "1";
				create.type = Constants.TYPE_CONTENT;
				create.property = properties;

				// Create and execute the cml statement
				CML cml = new CML();
				cml.create = new CMLCreate[] { create };
				UpdateResult[] updateResult = repoService.update(cml);

				// work around to cast Alfresco.RepositoryWebService.Reference to
				// Alfresco.ContentWebService.Reference 
				Alfresco.RepositoryWebService.Reference rwsRef = updateResult[0].destination;
				Alfresco.ContentWebService.Reference newContentNode = new Alfresco.ContentWebService.Reference();
				newContentNode.path = rwsRef.path;
				newContentNode.uuid = rwsRef.uuid;
				Alfresco.ContentWebService.Store cwsStore = new Alfresco.ContentWebService.Store();
				cwsStore.address = "SpacesStore";
				spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
				newContentNode.store = cwsStore;

				// Open the file and convert to byte array 
				FileStream inputStream = new FileStream(file, FileMode.Open);

				int bufferSize = (int)inputStream.Length;
				byte[] bytes = new byte[bufferSize];
				inputStream.Read(bytes, 0, bufferSize);

				inputStream.Close();

				Alfresco.ContentWebService.ContentFormat contentFormat = new Alfresco.ContentWebService.ContentFormat();
				contentFormat.mimetype = mimeType.GuessMimetype(file);

				WebServiceFactory.getContentService().write(newContentNode, Constants.PROP_CONTENT, bytes, contentFormat);

				// Reset the cursor to the default for all controls.
				Cursor.Current = Cursors.Default;

				MessageBox.Show(file + " uploaded");

			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				MessageBox.Show(ex.StackTrace);
			}
		}

		private void btnLocation_Click(object sender, EventArgs e)
		{


			if (this.browse == null || this.browse.IsDisposed)
			{
				this.browse = new TreeViewBrowse();
				// set the parent form
				this.browse.Owner = this;
				this.browse.RepoService = this.repoService;
				this.browse.Show();
			}


		}
	}

}