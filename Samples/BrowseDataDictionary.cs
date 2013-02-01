using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alfresco;
using Alfresco.DictionaryServiceWebService;

namespace Samples
{
    public partial class BrowseDataDictionary : Form
    {
        public BrowseDataDictionary()
        {
            InitializeComponent();
        }

        private void BrowseDataDictionary_Load(object sender, EventArgs e)
        {
            AuthenticationUtils.startSession("admin", "sametsis");
            try
            {
                ClassDefinition[] classDefinitions = WebServiceFactory.getDictionaryService().getClasses(null, null);
                foreach (ClassDefinition classDefinition in classDefinitions)
                {
                    string displayLabel = classDefinition.title;
                    if (displayLabel != null && displayLabel.Trim().Length != 0)
                    {
                        ListViewItem item = new ListViewItem(classDefinition.title);
                        item.Tag = classDefinition;
                        listBoxClasses.Items.Add(item);

                        listBoxClasses.DisplayMember = "Text";
                        listBoxClasses.ValueMember = "Tag";
                    }
                }
            }
            finally
            {
                AuthenticationUtils.endSession();
            }
        }

        private void listBoxClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem item = (ListViewItem)listBoxClasses.Items[listBoxClasses.SelectedIndex];
            ClassDefinition classDefinition = (ClassDefinition)item.Tag;

            // Indicate whether we are looking at the details of an aspect or type
            if (classDefinition.isAspect == false)
            {
                groupBoxDetails.Text = "Type Details";
            }
            else
            {
                groupBoxDetails.Text = "Aspect Details";
            }

            // Set the values of the various labels
            labelSuperClass.Text = classDefinition.superClass;
            labelTitle.Text = classDefinition.title;
            labelName.Text = classDefinition.name;
            labelDescription.Text = classDefinition.description;
        }

    }
}