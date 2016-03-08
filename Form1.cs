using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using MediaInfoLib;
using MediaBrowser.Library.Entities;
using System.Reflection;
using HolyOneMediaInfo;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        ListViewColumnSorter lvwColumnSorter = null;
        public Form1()
        {
        

            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
					
        }



        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
          ChoseDirs chd = new ChoseDirs(this  );
         chd.ShowDialog();
         if (listView1.Items.Count == 0) return;
         listView1.BeginUpdate();
         try
         {
         foreach (ColumnHeader item in listView1.Columns)
         {
             item.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
         }
         }
         finally
         {


         listView1.EndUpdate();
         toolStripStatusLabel4.Text = listView1.Items.Count.ToString() + " Items Total";
         }


        }


        Dictionary<string, ListViewGroup> sg = new Dictionary<string, ListViewGroup>();
        void Additem2(Media m)
        {
            string s=m.Filename;

                 string  g = m.GroupName;
                      string[] extrafiles = m._ExtraFiles;
                      listView1.BeginUpdate();
                      try
                      {

            ListViewItem lvi = listView1.Items.Add(System.IO.Path.GetFileName(s));

           

            if (!String.IsNullOrEmpty(g))
            {
              
                ListViewGroup grp = null;
                if (sg.ContainsKey(g)) grp = sg[g];
                else
                {

                    grp = new ListViewGroup(m.GroupName, m.GroupName);
                    sg.Add(g, grp);
                    listView1.Groups.Add(grp);
                }
            
                lvi.Group = grp;
                
            if( m.FileType=="Video") 
                lvi.ImageIndex = 0;else 
            if (m.FileType == "Audio") 
                lvi.ImageIndex = 1;
            else lvi.ImageIndex = 2;
            }
            
         //   if (listView1.Groups.Contains(  g != null) groupname = g ;



            //  lvi.SubItems.Add(m.Directory);

           
         
            //     lvi.SubItems.Add(extfilesstr);
            //     lvi.SubItems.Add(m.getSizeHumanReadable());



            Type t = m.GetType();
            PropertyInfo[] pi = t.GetProperties();
            foreach (PropertyInfo prp in pi)
            {

                object val = prp.GetValue(m, null);

                //string s = prp.GetValue(null, null);
                if (val == null) lvi.SubItems.Add(""); else
                  lvi.SubItems.Add(val.ToString());
            }
            lvi.Tag = m;
            lvi.Checked = m.Checked;

                      }
                      finally
                      {
                          listView1.EndUpdate();
                      }
        }
       public void  Open()
{
	OpenFileDialog openDlg = new OpenFileDialog();
	openDlg.Filter  = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
	openDlg.FileName = "" ;
	openDlg.DefaultExt = ".xml";
	openDlg.CheckFileExists = true;
	openDlg.CheckPathExists = true;
	DialogResult res = openDlg.ShowDialog ();
	if(res == DialogResult.OK)
	{
        if (!(openDlg.FileName).EndsWith(".xml") && !(openDlg.FileName).EndsWith(".xml"))
			MessageBox.Show("Unexpected file format","XML",MessageBoxButtons.OK );
		else
		{
	//  string str=System.IO.File.ReadAllText(openDlg.FileName);
      SaveFile sf = Functions.DeserializeXml<SaveFile>(openDlg.FileName);

   listView1.Items.Clear();
   listView1.Groups.Clear();
   sg.Clear();
   this.Text = Application.ProductName + " - " + System.IO.Path.GetFileName(openDlg.FileName) + " [" + sf.GenerateTime.ToShortDateString() + "]";
   listView1.BeginUpdate();
   listView1.SuspendLayout();
   try
   {
   foreach (Media   m in sf.Medias)
   {
       Additem2(m);
   }
   }
 finally
   {    listView1.ResumeLayout();
   listView1.EndUpdate();
   }




		}
	}
          
}
//Save the document
     
       public void Save()
       {

           SaveFileDialog saveDlg = new SaveFileDialog();
           saveDlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
           saveDlg.DefaultExt = ".xml";
           saveDlg.FileName = "Movielist.xml";
           DialogResult res = saveDlg.ShowDialog();
           if (res == DialogResult.OK)
           {
               SaveFile sf = new SaveFile();
               listView1.SuspendLayout();
               try
               {
        foreach (ListViewItem lvi in listView1.Items)
               {
                   Media m = (Media)lvi.Tag;
                   m.Checked = (lvi.Checked); 
                   sf.Medias.Add(m);
               }
               }
               finally
               {
                   listView1.ResumeLayout();
               }
                
               Functions.SerializeObject( sf,saveDlg.FileName);

             //  System.IO.File.WriteAllText(saveDlg.FileName,);

               //	selectedView.GetDocument().SaveDocument(saveDlg.FileName);

           }
       }

        private void Form1_Load(object sender, EventArgs e)
        {
            MediaBrowser.Library.Entities.Media m2 = new Media();
            Type t = m2.GetType();
            PropertyInfo[] pi = t.GetProperties();
            foreach (PropertyInfo prp in pi)
            {
             ColumnHeader hdr= listView1.Columns.Add( prp.Name);
              hdr.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize );//|  ColumnHeaderAutoResizeStyle.HeaderSize
            }

            }
 

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {

                MessageBox.Show("No media exists in list");
                return;
            }
            Save();
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
   
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetCheckAll(true );
        }

        private void button5_Click(object sender, EventArgs e)
        {
        
            SetCheckAll(false);
        }

        private void SetCheckAll(bool checkstate)
        {
            listView1.BeginUpdate();
            foreach (ListViewItem lvi in listView1.Items)
            {
                lvi.Checked = checkstate;

            }
            listView1.EndUpdate();
        }

        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {  
          if (listView1.SelectedItems.Count  == 0) return;

          ListViewItem lvi = listView1.SelectedItems[0];
          Media m = (Media)lvi.Tag;
            if(System.IO.Directory.Exists(m.Directory))
                System.Diagnostics.Process.Start(m.Directory);
            else MessageBox.Show("Directory dont exist.");

        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            string s = "";
             
            foreach (string file in files)
            {
             s+= (file + "\r\n");

            }
            ChoseDirs chd = new ChoseDirs(this,s);
            chd.ShowDialog();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                // allow them to continue
                // (without this, the cursor stays a "NO" symbol
                e.Effect = DragDropEffects.All;
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
           
           if(e.Item.Tag!=null) (e.Item.Tag as Media).Checked = e.Item.Checked;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {  listView1.BeginUpdate();
        try
        {
            Open();
            toolStripStatusLabel4.Text = listView1.Items.Count.ToString() + " Items Total";
            if (listView1.Items.Count == 0) return ;
          
            foreach (ColumnHeader  item in listView1.Columns)
            {
                item.AutoResize( ColumnHeaderAutoResizeStyle.ColumnContent);
            }

        }
        finally
        {
             
            listView1.EndUpdate();
        }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("No media exists in list");
                return;
            }
            folderBrowserDialog1.Description = "Select target to copy movies";
            if (folderBrowserDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK) return ;


            string targetpath = folderBrowserDialog1.SelectedPath;

            List<Media> mm =  listView1.Items
            .OfType<ListViewItem>().Where(X => X.Checked)
            .Select(X => (Media )X.Tag)
            .ToList();

            List<Media2> mm2 = new List<Media2>();
            foreach (Media  m in mm)
            {
                mm2.Add(new Media2 { SourceMedia = m, TargetDir = targetpath + m.Directory.Replace(":", "_DRV")});
             
            }

            StringBuilder sb = new StringBuilder();

            foreach (Media2 m2 in mm2)
            {
                sb.Append(m2.getBatch());
               
                sb.Append("\r\n");
            }
         // sb.Append("Pause\r\n");
            batchform b = new batchform(sb.ToString());
            b.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
      
        }
        string searchtxt = "";
        bool searchsubitems = false;
        ListViewItem LastFoundlvi = null;

        private ListViewItem FindItemByText2(ListView lstView, string SearchText, bool includeSubItemsInSearch, int StartIndex)
        {
         //   ListViewItem lvi = null;
            int colcount = 0;// int index = 0;
            if (includeSubItemsInSearch == true) colcount = lstView.Columns.Count;
            else
                colcount = 1;
            if (StartIndex > lstView.Items.Count)
                StartIndex = lstView.Items.Count;

            listView1.SuspendLayout();
            try
            {
            for (int j = 0; j < colcount; j++)
            {
                for (int i = StartIndex; i < lstView.Items.Count; i++)
                {
                    if ( lstView.Items[i].SubItems[j].Text.IndexOf(  SearchText, StringComparison.InvariantCultureIgnoreCase ) >=0)
                    {

                        return lstView.Items[i];
                        
                    }
                }
               // if (index != 0) break;
            }


            }
            finally
            {
                listView1.ResumeLayout();
            }
          //  lstView.FullRowSelect = true;
          //  lstView.Items[index].Selected = true;
        //    lstView.Select();
            return null;
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F3 || ( e.Control  && e.KeyCode== Keys.F) )
            {
             //   if ((e.Control && e.KeyCode == Keys.F)) searchtxt = "";

                if (searchtxt == "" || (e.Control && e.KeyCode == Keys.F))
                {
                    FindDlg fd = new FindDlg();
                    fd.textBox1.Text = searchtxt;
                    fd.textBox1.SelectAll();
                    fd.ShowDialog();
                    if (fd.DialogResult != System.Windows.Forms.DialogResult.OK) return;
                    searchtxt = fd.textBox1.Text;
                    searchsubitems = fd.checkBox1.Checked;
                    fd = null;
                }
                int startitem = 0;
                if (LastFoundlvi != null)
                    startitem = LastFoundlvi.Index;
               // if(listView1.SelectedItems.Count>0)    startitem = listView1.SelectedItems[0].Index;

              //  LastFoundlvi = listView1.FindItemWithText(searchtxt, searchsubitems, startitem+1, true  );
   // if (LastFoundlvi == null) LastFoundlvi = listView1.FindItemWithText(searchtxt, searchsubitems, 0, true);

             LastFoundlvi =   FindItemByText2( listView1 , searchtxt, searchsubitems, startitem+1   );
            if (LastFoundlvi == null) LastFoundlvi = FindItemByText2(listView1, searchtxt, searchsubitems,0);

            
                if (LastFoundlvi != null)
                {
                
                    listView1.SelectedItems.Clear();
                        listView1.TopItem = LastFoundlvi;
                    LastFoundlvi.Selected = true;

                }
                else MessageBox.Show("Can't find "+searchtxt);
                listView1.Select();
            } else
            if (e.Control && e.KeyCode == Keys.A)
            {
                NativeMethods.SelectAllItems(listView1);

            }else

            if (e.KeyCode == Keys.Delete)
            {
                if (listView1.Items.Count > 0)
                {
                    deleteToolStripMenuItem_Click(sender, null);
                    refreshItemCounts();
                }
            }

        }

        private void refreshItemCounts()
        {
            toolStripStatusLabel1.Text = listView1.SelectedItems.Count.ToString() + " items selected";
            long sz = 0;
            try
            {
                if (listView1.SelectedItems.Count < 1250)
                {
                    listView1.SuspendLayout();

                    foreach (ListViewItem lvi in listView1.SelectedItems)
                    {
                        sz += ((Media)(lvi.Tag))._Size;

                    }
                    listView1.ResumeLayout();
                    toolStripStatusLabel2.Text = Functions.StrFormatByteSize(sz);
                }else  toolStripStatusLabel2.Text = "Too many files selected, cant calculate total size";
            }
            catch (Exception)
            {
                toolStripStatusLabel2.Text = "-";
                throw;
            }

        }

        private void playMovieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            ListViewItem lvi = listView1.SelectedItems[0];
            Media m = (Media)lvi.Tag;
            string fn = m.Directory + "\\" + m.Filename;
            if(System.IO.File.Exists(fn))
            System.Diagnostics.Process.Start(fn); else
                MessageBox.Show("File dont exist.");
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            this.listView1.Sort();
			

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshItemCounts();
           
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = Functions.GetDefaultBrowserPath();
            p.StartInfo.Arguments = "http://www.tahribat.com";
            p.Start();

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (System.Windows.Forms.MessageBox.Show(string.Format("Are you sure you want to delete {0} items from the list?", listView1.SelectedItems.Count), "Are you sure?", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //  List<int> ii = new List<int>();

                //for (int i = this.listView1.SelectedItems.Count - 1; i >= 0; i--)
                //  this.listView1.Items.Remove(this.listView1.SelectedItems[0]);
                listView1.BeginUpdate();
                int indent = 0;
                foreach (int sli in listView1.SelectedIndices)
                {



                    listView1.Items.RemoveAt(sli - indent++);
                }
                listView1.EndUpdate();



                /*
                for (int i = ii.Count; i >= 0; i--) 
                {
                    listView1.Items.RemoveAt(i);
                }*/
                refreshItemCounts();
                toolStripStatusLabel4.Text = listView1.Items.Count.ToString() + " Items Total";
            }
        }

        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
           
            foreach (ListViewItem sli in listView1.SelectedItems)
            {
                sli.Checked = true;
            }
            listView1.EndUpdate();

        }

        private void unCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.BeginUpdate();

            foreach (ListViewItem sli in listView1.SelectedItems)
            {
                sli.Checked = false ;
            }
            listView1.EndUpdate();
        }
        long totalchecked = 0;
        private void listView1_ItemChecked_1(object sender, ItemCheckedEventArgs e)
        {


          /*
                long sz = 0;
                try
                {

                    listView1.SuspendLayout();
                    foreach (ListViewItem lvi in listView1.CheckedItems)
                    {
                        sz += ((Media)(lvi.Tag))._Size;
                    }
                   
                    toolStripStatusLabel4.Text = "Checked Items Size:" + Functions.StrFormatByteSize(sz);
                }
                finally
                { 
                 listView1.ResumeLayout();
                
                }*/
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            //     toolStripStatusLabel1.Text = "Selected "+ listView1.SelectedItems.Count.ToString()+" Items";
            
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
               
                if (listView1.SelectedItems.Count > 0)
                {
                    contextMenuStrip1.Tag = listView1.SelectedItems[0].Tag;
                    Point p = new Point();
                    p.X = e.X;
                    p.Y = e.Y;

                    contextMenuStrip1.Show(sender as Control, p);
                    //...
                }
            }
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            ListViewItem lvi = listView1.SelectedItems[0];
            Media m = (Media)lvi.Tag;
            string fn =   m.Filename;
          
            {
                string fn2=fn;
                string[] reps = new string[] {
                "divx","ac3","dvdrip","bdrip","bluray","x264","hdtv","720p" , "1080p", "720i","1080i","3dtv","360p", "'","xvid","mpeg","dts"
                };
                fn2 = System.IO.Path.GetFileNameWithoutExtension(fn2);
                fn2 = fn2.Replace('.', ' ');
                fn2 = fn2.Replace('-', ' ');
                fn2 = fn2.Replace('+', ' ');
                fn2 = fn2.ToLower();

                foreach (string  s in reps)
                {
                    fn2 = fn2.Replace(s, "");
                }
               
                fn2 = fn2.Trim();

                if (string.IsNullOrEmpty(fn2)) fn2 = fn;

                try
                {

        
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = Functions.GetDefaultBrowserPath();


                SearchImdb.Imdb im = new SearchImdb.Imdb();
                im.Search(fn2);
                if ( string.IsNullOrEmpty(im.Link))
                {

                    p.StartInfo.Arguments = "http://www.imdb.com/find?s=all&q=" + fn2;
                  
                }
                else
                {
                    p.StartInfo.Arguments = im.Link;
                
                }
  p.Start();
                }
                catch (Exception)
                {

                    throw;
                }
            }
 
        }

        private void button7_Click_1(object sender, EventArgs e)
        {


        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                listView1_KeyDown(sender, e);
           // else if (  e.KeyCode == Keys.F3)    listView1_KeyDown(sender, e);

            if (e.Control && e.KeyCode == Keys.S)
            {
                button3_Click(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.O)
            {
                button1_Click_1(sender, e);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}
