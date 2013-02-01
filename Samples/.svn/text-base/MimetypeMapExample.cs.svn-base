using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alfresco;

namespace Samples
{
    public partial class MimetypeMapExample : Form
    {
        public MimetypeMapExample()
        {
            InitializeComponent();
        }

        // Opens a file select dialoge box
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Use the file dialog to get a file name
                String file = openFileDialog1.FileName;                
                this.textBox2.Text = file;

                // Guess the mimetype from the file name and show in a message box
                String mimetype = MimetypeMap.Instance.GuessMimetype(file);
                String display = MimetypeMap.Instance.DisplaysByMimetype[mimetype];
                MessageBox.Show(display + " (" + mimetype + ")");
            }

        }
    }
}