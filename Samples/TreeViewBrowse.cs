using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alfresco;
using Alfresco.RepositoryWebService;

namespace Samples
{
	public partial class TreeViewBrowse : Form
	{
		private Alfresco.RepositoryWebService.Store spacesStore;
		private RepositoryService repoService;
		Alfresco.RepositoryWebService.Reference reference;

		public RepositoryService RepoService
		{
			set { repoService = value; }
		}

		public TreeViewBrowse()
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
			this.initializeRootFolder();
		}

		private void initializeRootFolder()
		{

			// Suppress repainting the TreeView until all the objects have been created.
			treeView1.BeginUpdate();

			// get the root position, Company Home

			TreeNode rootNode = new TreeNode();

			Alfresco.RepositoryWebService.Reference reference = new Alfresco.RepositoryWebService.Reference();
			reference.store = this.spacesStore;
			reference.path = "/app:company_home";

			// Create a query object
			Query query = new Query();
			query.language = Constants.QUERY_LANG_LUCENE;
			query.statement = "Path:\"/\" AND @cm\\:title:\"Company Home\"";

			QueryResult result = this.repoService.query(this.spacesStore, query, true);
			string name = null;
			if (result.resultSet.rows != null)
			{
				// construct root node
				ResultSetRow row = result.resultSet.rows[0];
				foreach (NamedValue namedValue in row.columns)
				{
					if (namedValue.name.Contains("title") == true)
					{
						name = namedValue.value;
						rootNode.Text = name;
						rootNode.Name = name;
					}
				}
				rootNode.Tag = row.node;
			}
			// add the root node to the tree view
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] { rootNode });

			buildTree(rootNode, reference);

			// Begin repainting the TreeView.
			treeView1.EndUpdate();

		}

		/// <summary>
		/// Constructs and adds a child node to the tree view at the parentNode supplied
		/// </summary>
		private TreeNode addChildNode(TreeNode parentNode, String name, ResultSetRowNode rsrNode)
		{

			TreeNode node = new TreeNode(name);
			node.Text = name;
			node.Tag = rsrNode;
			parentNode.Nodes.Add(node);
			return node;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			ResultSetRowNode node = (ResultSetRowNode)treeView1.SelectedNode.Tag;
			// get the parent form
			UploadExample parentForm = (UploadExample)this.Owner;
			parentForm.LocationUuid = node.id;
			parentForm.LocationName = treeView1.SelectedNode.Text;

			this.Close();

		}
		// load the treeView
		private void buildTree(TreeNode parentNode, Reference childReference)
		{

			// Query for the children of the reference
			QueryResult result = this.repoService.queryChildren(childReference);
			if (result.resultSet.rows != null)
			{
				foreach (ResultSetRow row in result.resultSet.rows)
				{
					// only interested in folders
					if (row.node.type.Contains("folder") == true)
					{
						foreach (NamedValue namedValue in row.columns)
						{
							if (namedValue.name.Contains("name") == true)
							{
								// add a node to the tree view
								TreeNode node = this.addChildNode(parentNode, namedValue.value, row.node);

								// Create the reference for the node selected
								Alfresco.RepositoryWebService.Reference reference = new Alfresco.RepositoryWebService.Reference();
								reference.store = this.spacesStore;
								reference.uuid = row.node.id;
								// add the child nodes
								buildTree(node, reference);
							}
						}
					}
				}
			}
		}


	}
}