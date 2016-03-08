using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace HolyOneMediaInfo
{
    public partial class batchform : Form
    {
        public batchform( string str)
        {
            InitializeComponent();
            richTextBox1.Text = str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tmp= System.IO.Path.GetTempFileName()+".bat";
            System.IO.File.WriteAllText(tmp,"@echo off\r\n"+richTextBox1.Text+"\r\nPause");

            using (Process p = Process.Start(tmp))
            {
                while (!p.HasExited)
                {
                  //  Console.WriteLine("Waiting...");
                    Thread.Sleep(1000);
                }
            }

            System.IO.File.Delete(tmp);

            MessageBox.Show("Batch Job Finished");
            Close();
        }

        private void batchform_Load(object sender, EventArgs e)
        {

        }
    }
}
