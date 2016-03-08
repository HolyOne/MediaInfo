using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HolyOneMediaInfo
{
    public partial class FindDlg : Form
    {
        public FindDlg()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Trim();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void FindDlg_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                button2_Click(sender, null);
        }

        private void FindDlg_Shown(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void FindDlg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) 
            {
                button1_Click(sender, e);
                 
            }
        }
    }
}
