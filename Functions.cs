using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaInfoLib;
using MediaBrowser.Library.Entities;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Xml;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing;
using System.Reflection;

namespace WindowsFormsApplication2
{
   public static  class Functions
    {


        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatByteSize(
                long fileSize
                , [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer
                , int bufferSize);


        /// <summary>
        /// Converts a numeric value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
        /// </summary>
        /// <param name="filelength">The numeric value to be converted.</param>
        /// <returns>the converted string</returns>
        public static string StrFormatByteSize(long filesize)
        {
            StringBuilder sb = new StringBuilder(11);
            StrFormatByteSize(filesize, sb, sb.Capacity);
            return sb.ToString();
        }


        public static string GetFileSizeString(string filePath)
        {
            try
            {
                FileInfo info = new FileInfo(filePath);

                return StrFormatByteSize(info.Length);
            }
            catch (Exception)
            {

                return "Err";
            }

        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        public static Media GetMediaInfo(string location, string groupname, string  formattype,string[] extrafiles = null)
        {
            Console.WriteLine("getting media info from " + location);




            MediaInfo mediaInfo = new MediaInfo();

 
           
            int i = mediaInfo.Open(location);


            Media mediaInfoData = null;

    

            if (i != 0)
            {
                int width;
                Int32.TryParse(mediaInfo.Get(StreamKind.Video, 0, "Width"), out width);
                int height;
                Int32.TryParse(mediaInfo.Get(StreamKind.Video, 0, "Height"), out height);
                int videoBitRate;
                Int32.TryParse(mediaInfo.Get(StreamKind.Video, 0, "BitRate"), out videoBitRate);
                int audioBitRate;
                Int32.TryParse(mediaInfo.Get(StreamKind.Audio, 0, "BitRate"), out audioBitRate);
                int runTime;
                Int32.TryParse(mediaInfo.Get(StreamKind.General, 0, "PlayTime"), out runTime);
                int streamCount;
                Int32.TryParse(mediaInfo.Get(StreamKind.Audio, 0, "StreamCount"), out streamCount);
                int audioChannels;
                Int32.TryParse(mediaInfo.Get(StreamKind.Audio, 0, "Channel(s)"), out audioChannels);
                string subtitles = mediaInfo.Get(StreamKind.General, 0, "Text_Language_List");
                string videoFrameRate = mediaInfo.Get(StreamKind.Video, 0, "FrameRate/String");


                mediaInfoData = new Media
                {
                    VideoCodec = mediaInfo.Get(StreamKind.Video, 0, "Codec/String"),
                    VideoBitRate = videoBitRate,
                    //MI.Get(StreamKind.Video, 0, "DisplayAspectRatio")),
                    Height = height,
                    Width = width,
                    //MI.Get(StreamKind.Video, 0, "Duration/String3")),
                    AudioFormat = mediaInfo.Get(StreamKind.Audio, 0, "Format"),
                    AudioBitRate = audioBitRate,
                    RunTime = (runTime / 60000),
                    AudioStreamCount = streamCount,
                    AudioChannelCount = audioChannels,
                    AudioProfile = mediaInfo.Get(StreamKind.Audio, 0, "Format_Profile"),
                    VideoFPS = videoFrameRate,
                    Subtitles = subtitles
                };

                mediaInfoData.GroupName = groupname;
                mediaInfoData.Filename = System.IO.Path.GetFileName(location);
                mediaInfoData.Directory = System.IO.Path.GetDirectoryName(location);
                mediaInfoData.Extension = System.IO.Path.GetExtension(location);
                mediaInfoData._ExtraFiles = extrafiles;
                mediaInfoData.FileType = formattype;

                FileInfo f = new FileInfo(location);
                mediaInfoData._Size = f.Length;

            }
            else
            {
                Console.WriteLine("Could not extract media information from " + location);
            }
            mediaInfo.Close();
            return mediaInfoData;
        }


        private static  void SerializeWithStylesheet(object 
                dictionary,  string filename, string stylesheet)
        {
            
            // First serialize the document to an in-memory stream
            MemoryStream memXmlStream = new MemoryStream();

            XmlSerializer serializer =
                new XmlSerializer(typeof(SaveFile));

            serializer.Serialize(memXmlStream, dictionary);

            // Now create a document with
            // the stylesheet processing instruction
            XmlDocument xmlDoc = new XmlDocument();
            
            // Now load the in-memory stream
            // XML data into the XMl document
            memXmlStream.Seek(0, SeekOrigin.Begin);
            xmlDoc.Load(memXmlStream);

            // Now add the stylesheet processing
            // instruction to the XML document
            XmlProcessingInstruction newPI;

            if (!string.IsNullOrEmpty(stylesheet))
            {


                string filepath = System.IO.Path.GetDirectoryName(filename)+"\\";
                string xslfilename=filepath+stylesheet;

                if (!System.IO.File.Exists(xslfilename)) 
                {

                    Assembly Assemb = Assembly.GetExecutingAssembly();
                    Stream XSLstream = Assemb.GetManifestResourceStream("HolyOneMediaInfo.MediaInfo.xsl");
                    XSLstream.Seek(0, SeekOrigin.Begin);

                    FileStream fs = new FileStream(xslfilename, FileMode.Create);
                    try
                    {
                        StreamReader Reader = new StreamReader(XSLstream);
                        StreamWriter Writer = new StreamWriter(fs);
                        try
                        {
                            Writer.Write(Reader.ReadToEnd());
                        }
                        finally
                        {
                            Writer.Close();
                        }

                        Reader.Close();


                    }
                    finally
                    {
                        fs.Close();

                    }


                }

                String PItext = string.Format(@"type=""text/xsl"" href=""{0}""", stylesheet);
                newPI = xmlDoc.CreateProcessingInstruction("xml-stylesheet", PItext);

                xmlDoc.InsertAfter(newPI, xmlDoc.FirstChild);
            }
            // Now write the document
            // out to the final output stream
            FileStream stream = new FileStream(filename, FileMode.Create );

            try
            {
           XmlTextWriter xmlWriter = new XmlTextWriter(stream,
                                      System.Text.Encoding.UTF8);
           xmlWriter.Formatting = Formatting.Indented;
            xmlDoc.WriteTo(xmlWriter);
            xmlWriter.Flush();
            }
            finally
            {
stream.Close();
            }
 
            


        }


        public static  void SerializeObject(Object pObject,string filename)
        {

            try
            {
               SerializeWithStylesheet(pObject, filename, "MediaInfo.xsl");
             /*
                XmlSerializer xs = new XmlSerializer(pObject.GetType());
              
                StreamWriter myWriter = new StreamWriter(filename );
    
            
                xs.Serialize(myWriter, pObject);
                myWriter.Close();*/
              
            }

            catch (Exception e)
            {
                MessageBox.Show("Error:"+ e.Message );
               
                return  ;
            }

        }

        public  static  T DeserializeXml<T>(string XmlData)
        {
            // Yeni bir Personel Nesnesi YaratÄ±yoruz Personel ReturnedEmployee; 
            // DeSerialize iÅŸleminde kullanÄ±lmak Ã¼zere yeni bir XmlSerializer 
            // nesnesi yaratÄ±yor ve Serialize edilmiÅŸ verinin hangi nesne(Class) tipine Ã§evirileceÄŸini gÃ¶steriyoruz. 
            XmlSerializer MyDeserializer = new XmlSerializer(typeof(T));

            try
            {
          FileStream myFileStream = new FileStream(XmlData, FileMode.Open);
            T x=   (T)MyDeserializer.Deserialize(myFileStream);
            myFileStream.Close();
            return x;
            }
            catch (Exception x)
            {
                 throw new Exception("Cant Deserialise\r\n"+ x.Message );
               
            }
  
            /*

            // XML Verisini tutmak iÃ§in bir StringReader yaratÄ±yoruz.  
            StringReader SR = new StringReader(XmlData);

            XmlReader XR = new XmlTextReader(SR);

            // XML verisinin Deserialize edilip edilmeyeceÄŸini kontrol ediyoruz.  
         
            if (MyDeserializer.CanDeserialize(XR))
            {
                // Ve XML verisini Deserialize ediyoruz. 
                T ReturnedEmployee = (T)MyDeserializer.Deserialize(XR);
                return ReturnedEmployee;
                //  ShowEmployeeData(ReturnedEmployee);
            }
            else
            */
          
        }


        /// <returns></returns>

        public static string GetDefaultBrowserPath()
        {

            string key = @"htmlfile\shell\open\command";
            RegistryKey registryKey =
            Registry.ClassesRoot.OpenSubKey(key, false);
            // get default browser path
            return ((string)registryKey.GetValue(null, null)).Split('"')[1];

        }
 

    }
}
