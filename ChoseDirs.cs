using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaBrowser.Library.Entities;
using MediaInfoLib;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.Win32;

namespace WindowsFormsApplication2
{
    public partial class ChoseDirs : Form
    {
        Form1 mainf = null;
        public ChoseDirs(Form1 form1)
        {
            mainf = form1;



            InitializeComponent();
            LoadReg();

        }
//---
        private void LoadReg(bool loaddirs=true)
        {
            try
            {
                LoadCustom();
                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.CreateSubKey("Software\\HolyOne\\MovieCollection");

              if(loaddirs)  textBox1.Text = (string)regKey.GetValue("LastLoadedDirectories", "");

                string[] lastchecked = ((string)regKey.GetValue("LastCheckedFileTypes", "")).Split(";".ToCharArray());
                foreach (ListViewItem lvi in listView1.Items)
                    if (lastchecked.Contains(lvi.Text.ToLowerInvariant())) lvi.Checked = true;



                // regKey.SetValue("CurrColor", "Red");
                Console.WriteLine("Settings saved in registry");
                regKey.Close();
            }
            catch
            {

            }
        }


        public ChoseDirs(Form1 form1,string dirsvalue)
        {
            mainf = form1;
            InitializeComponent();
            LoadReg(false);
            textBox1.Text = dirsvalue;
         //   button1_Click(null, null);
         //   this.ShowDialog( form1);
        }



        void DirSearch(ListViewGroup g, string sDir)
        {
 


            if (stp) return ;
            try
            {

                label2.Text = "Processing:" + sDir;
                Application.DoEvents();
                foreach (formatinfo ext in arrextensions)

                    foreach (string f in Directory.GetFiles(sDir, ext.extension))
                { if (stp) break;
                    List<string> extrafiles = new List<string>();
                    string namewithoutext = System.IO.Path.GetFileNameWithoutExtension(f);
                    string extension = System.IO.Path.GetExtension(f);

                    if (ext.groupname == "Video") 

                    foreach (string n in Directory.GetFiles(sDir,namewithoutext+ ".*"))
                    {
                        if (n == f) continue;
            
                            extrafiles.Add( System.IO.Path.GetFileName( n));
                    

                    }


                    Additem(f, g,ext.groupname , extrafiles.ToArray());

                }
               // mainf.listView1.BeginUpdate();
                foreach (string d in Directory.GetDirectories(sDir))
                {

                    DirSearch(g, d);
                }

             //   mainf.listView1.EndUpdate();

            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

        }
        formatinfo[] arrextensions = null;
        public class formatinfo
        {
            public string extension;
            public string groupname;

        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (listView1.CheckedItems.Count == 0)
            {

                MessageBox.Show("Select some media types to start searching.");
            

                return;
            }
            counter = 0;
             
   
            if (checkBox1.Checked)
            {
                mainf.listView1.Groups.Clear();
                mainf.listView1.Items.Clear();
            }
            stp = false;
            try
            {
                SaveCustom();
                listView1.Enabled = false ;
                textBox1.Enabled = false;
                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.CreateSubKey("Software\\HolyOne\\MovieCollection");
               
                regKey.SetValue("LastLoadedDirectories", textBox1.Text);
               

               List<string> MyList=listView1.Items
               .OfType<ListViewItem>().Where( X=> X.Checked)
               .Select(X => X.Text.ToLowerInvariant().Trim())
               .ToList();


               string LastCheckedFileTypes = string.Join(";", MyList);

                regKey.SetValue("LastCheckedFileTypes", LastCheckedFileTypes);
             
               // regKey.SetValue("CurrColor", "Red");
                Console.WriteLine("Settings saved in registry");
 regKey.Close();


 List<formatinfo> formatinfos = new List<formatinfo>();
 foreach (ListViewItem  lvi in listView1.Items)
 {
   
     if (lvi.Checked) {
         formatinfos.Add  (  new formatinfo { extension= lvi.ToolTipText, groupname=lvi.Group.Name});
     }
 }
 arrextensions = formatinfos.ToArray();



                button2.Visible = true;
                button1.Visible = false;

            foreach (string s in textBox1.Lines)
            {
                string dirname = s.Trim();
                if (string.IsNullOrEmpty(dirname)) continue; 
                if (string.IsNullOrEmpty(dirname)) continue;
                if (dirname.Length == 2) if (dirname.EndsWith(":")) dirname += "\\";
                label2.Text = "Processing:" + dirname;
                Application.DoEvents();


                ListViewGroup g = mainf.listView1.Groups.Add(dirname, dirname);

                if ((!System.IO.Directory.Exists(dirname)) )
                   if (!System.IO.File.Exists(dirname)) continue;
                DirSearch(g, dirname);


            }
            }
            finally
            {
                listView1.Enabled = true;
                textBox1.Enabled = true;
                button2.Visible = false;
                button1.Visible = true;
            }
     
    
            Close();
        }

        int counter = 0;
        void Additem(string s, ListViewGroup g = null, string formattype="",  string[] extrafiles = null)
        {
        //    mainf.listView1.BeginUpdate();
            try
            {

            ListViewItem lvi = mainf.listView1.Items.Add(System.IO.Path.GetFileName(s));
            label4.Text = (++counter).ToString()+ " items found.";
            lvi.Group = g;

      string groupname="";
      if (g != null)
      {
          groupname = g.Name;
        
      }

      Media m = Functions.GetMediaInfo(s, groupname, formattype,extrafiles);
          

          if (formattype == "Video") lvi.ImageIndex = 0;
          else
              if (formattype == "Audio") lvi.ImageIndex = 1;
              else lvi.ImageIndex = 2;

 

            Type t = m.GetType();
            PropertyInfo[] pi = t.GetProperties();
            foreach (PropertyInfo prp in pi)
            {
                
                object val =  prp.GetValue(m, null);

                //string s = prp.GetValue(null, null);
                if(val!=null)
                lvi.SubItems.Add(val.ToString());else
                    lvi.SubItems.Add("");
            }
            lvi.Tag = m;
       //     mainf.treeView1.Nodes.Add(m.GroupName);

            }
            finally
            {
   
            }

 

        }


        private void Form2_Load(object sender, EventArgs e)
        {
          //  LoadCustom();
        }

        private void LoadCustom()
        {
            try
            {
                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.CreateSubKey("Software\\HolyOne\\MovieCollection");
                string readstr=(string)regKey.GetValue("CustomExtensions", "");
                if(String.IsNullOrEmpty(readstr))return;
                string[] exts = (readstr).Split("|".ToCharArray());

                foreach (string s in exts)
                {
                    ListViewItem lvi = new ListViewItem(s);
                    lvi.Group = listView1.Groups["Custom"];
                    lvi.ToolTipText = "*." + s;
                    listView1.Items.Add(lvi);
                }



                 
                regKey.Close();
            }
            catch
            {

            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // loop through the string array, adding each filename to the ListBox
            if(!textBox1.Text.EndsWith("\r\n"))  textBox1.AppendText("\r\n");
            foreach (string file in files)
            {
                textBox1.AppendText(file+"\r\n");
               
            }
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
           if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                // allow them to continue
                // (without this, the cursor stays a "NO" symbol
                e.Effect = DragDropEffects.All;
        }

        bool stp = false;
        private void button2_Click(object sender, EventArgs e)
        {
            stp = true;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ChoseDirs_FormClosing(object sender, FormClosingEventArgs e)
        {
            stp=true;

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string myval = null;
            if (DialogResult.Cancel == Functions.InputBox("Input new extension", "Input extension (example: mp3)", ref myval)) myval = null;

            if (myval == null) return;
            myval=myval.Trim();
            if (myval == "")
            {

                MessageBox.Show("Extension is empty", "Error" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (myval.LastIndexOfAny((@"|*/.?\""".ToCharArray())) >=0)
            {

                MessageBox.Show("Invalid character in file extension","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

           ListViewItem lvi = new ListViewItem(myval);
            lvi.ToolTipText = "*." + myval;

            lvi.Group = listView1.Groups["Custom"];


            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].ToolTipText == lvi.ToolTipText)
                {
                    MessageBox.Show("This extension already exists in extensions list", "Error",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }


            listView1.Items.Add(lvi);

            
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    if (listView1.SelectedItems[0].Group.Name == "Custom")
                    { 
                    
                    if( DialogResult.OK== MessageBox.Show("Delete this item?", "Delete "+ listView1.SelectedItems[0].Text +"?",  MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1))
                        listView1.Items.Remove(listView1.SelectedItems[0]);
                    
                    } else
                        MessageBox.Show("Only items in Custom group can be deleted.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
                
                }
            
            }
        }

        private void ChoseDirs_FormClosed(object sender, FormClosedEventArgs e)
        {

            SaveCustom();
           
        }

        private void SaveCustom()
        {
            try
            {
                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.CreateSubKey("Software\\HolyOne\\MovieCollection");




                List<string> MyList = listView1.Items
                .OfType<ListViewItem>().Where(X => X.Group.Name == "Custom")
                .Select(X => X.Text.ToLowerInvariant().Trim())
                .ToList();


                string CustomTypes = string.Join("|", MyList);

                regKey.SetValue("CustomExtensions", CustomTypes);
                regKey.Close();
            }
            catch
            {

            }
        }
    }
}
