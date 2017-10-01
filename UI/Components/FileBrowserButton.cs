using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeOverload.Windows.UI.Components
{
    public partial class FileBrowserButton : UserControl
    {
        public FileBrowserButton()
        {
            InitializeComponent();
        }

        private void FileBrowse_Click(object sender, EventArgs e)
        {
            if(Browser.ShowDialog() == DialogResult.OK)
            {
                FilePath.Text = Browser.FileName;
            }
        }
    }
}
