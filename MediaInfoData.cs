using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication2;
using System.Xml.Serialization;
 

namespace MediaBrowser.Library.Entities
{

    public class Media2 
    {
        public string TargetDir
        {
            get;  set;
          
        }
   //     public string[] ExtraFiles;
        public Media SourceMedia { get; set; }

        public string getBatch()
        {
            string dir = System.IO.Path.GetDirectoryName(TargetDir + SourceMedia.Directory.Substring(2));
               

            string cpycmd = @"xcopy """ + SourceMedia.Directory + (SourceMedia.Directory.EndsWith("\\") ? "" : "\\") + SourceMedia.Filename + @""" """
                  
              
                + dir
                + (dir.EndsWith("\\") ? "" : "\\")
                +  @"""" ;

            if (SourceMedia._ExtraFiles != null)
                foreach (string  item in SourceMedia._ExtraFiles)
                {
                    cpycmd += "\r\n\t" + @"xcopy """ + SourceMedia.Directory + (SourceMedia.Directory.EndsWith("\\") ? "" : "\\") + item + @""" """ 
                        +dir
                           + (dir.EndsWith("\\") ? "" : "\\")
                        + @"""";
                }
            return cpycmd;
        }
    }


    [Serializable]
    public class Media : MediaInfoData
    {
           [XmlAttribute()]
     public string  Filename { get; set; }
         [XmlAttribute()]
        public string Directory { get; set; }
         [XmlAttribute()]
        public string FileType { get; set; }

         [XmlAttribute()]
        public string Extension { get; set; }
      [XmlAttribute()]
        public string GroupName { get; set; }
         [XmlAttribute()]
        public long _Size { get; set; }
         [XmlArray( ElementName="ExtraFiles", IsNullable= false )]

        public string[] _ExtraFiles { get; set; }

        [XmlIgnore]
        public string ExtraFiles { get { return string.Join( "; ",_ExtraFiles );} }

        public bool Checked { get; set; }
        [XmlIgnore]
        public string SizeHumanReadable { get { return Functions.StrFormatByteSize(_Size); } }

        public Media()
        {
            Checked = false;
            _Size = 0;
        }
        public string getSizeHumanReadable()
        {
            return Functions.StrFormatByteSize(_Size);
        }
    }


    public class MediaInfoData
    {
        public readonly static MediaInfoData Empty = new MediaInfoData { AudioFormat = "", VideoCodec = "" };
          [XmlAttribute()]
        public int Height;
  [XmlAttribute()]
        public int Width;
  [XmlAttribute()]
        public string VideoCodec;
  [XmlAttribute()]
        public string AudioFormat;
  [XmlAttribute()]
        public int VideoBitRate;
  [XmlAttribute()]
        public int AudioBitRate;
   [XmlAttribute()]
        public int RunTime = 0;
   [XmlAttribute()]
        public int AudioStreamCount = 0;
   [XmlAttribute()]
        public int AudioChannelCount = 0;
   [XmlAttribute()]
        public string AudioProfile;
   [XmlAttribute()]
        public string Subtitles;

      [XmlAttribute()]

        public string VideoFPS;
        
      [XmlAttribute()]
        public string CombinedInfo
        {
            get
            {
                if (this != Empty)
                {
                    if (AudioProfile != "")
                        return string.Format("{0}x{1}, {2} {3}kbps, {4} ({5}) {6}kbps", this.Width, this.Height, this.VideoCodec, this.VideoBitRate / 1000, this.AudioFormat, this.AudioProfile, this.AudioBitRate / 1000);
                    else
                        return string.Format("{0}x{1}, {2} {3}kbps, {4} {5}kbps", this.Width, this.Height, this.VideoCodec, this.VideoBitRate / 1000, this.AudioFormat, this.AudioBitRate / 1000);
                }
                else
                    return "";
            }
        }

        #region Properties Video
          [XmlAttribute()]
        public string VideoResolutionString
        {
            get
            {
                if (this != Empty)
                    return string.Format("{0}x{1}", this.Width, this.Height);
                else
                    return "";
            }
        }
          [XmlAttribute()]
        public string VideoCodecString
        {
            get
            {
                if (this != Empty)
                    return string.Format("{0}", this.VideoCodec);
                else
                    return "";
            }
        }
          [XmlAttribute()]
        public string AspectRatioString
        {
            get
            {
                if (this != Empty)
                {
                    Single width = (Single)this.Width;
                    Single height = (Single)this.Height;
                    Single temp = (width / height);

                    if (temp < 1.4)
                        return "4:3";
                    else if (temp >= 1.4 && temp <= 1.55)
                        return "3:2";
                    else if (temp > 1.55 && temp <= 1.8)
                        return "16:9";
                    else if (temp > 1.8 && temp <= 2)
                        return "1.85:1";
                    else if (temp > 2)
                        return "2.39:1";
                    else
                        return "";
                }
                else
                    return "";
            }
        }
          [XmlAttribute()]
        public string RuntimeString
        {
            get
            {
                if (RunTime != 0)
                {
                    return RunTime.ToString() + " mins";
                }
                else return "";
            }
        }
          [XmlAttribute()]
        public string VideoFrameRateString
        {
            get
            {
                return VideoFPS;
            }
        }
          [XmlAttribute()]
        public string VideoCodecExtendedString
        {
            get
            {
                return string.Format("{0} {1}kbps", this.VideoCodec, this.VideoBitRate / 1000);
            }
        }
        #endregion

        #region Properties Audio
          [XmlAttribute()]
        public string AudioCodecString
        {
            get
            {
                if (this != Empty)
                    return string.Format("{0}", this.AudioFormat);
                else
                    return "";
            }
        }
          [XmlAttribute()]
        public string AudioChannelString
        {
            get
            {
                return AudioChannelCount.ToString();
            }
        }
          [XmlAttribute()]
        public string AudioStreamString
        {
            get
            {
                return AudioStreamCount.ToString();
            }
        }
          [XmlAttribute()]
        public string AudioCodecExtendedString
        {
            get
            {
                switch (this.AudioFormat.ToLower())
                {
                    case "ac3":
                    case "dts":
                    case "mpeg audio":
                        {
                            if (this.AudioProfile != null && this.AudioProfile != "")
                                return string.Format("{0} {1} {2}kbps", this.AudioFormat, this.AudioProfile, this.AudioBitRate / 1000);
                            else
                                return string.Format("{0} {1}kbps", this.AudioFormat, this.AudioBitRate / 1000);
                        }
                    default:
                        return string.Format("{0} {1}kbps", this.AudioFormat, this.AudioBitRate / 1000);
                }
            }
        }
          [XmlAttribute()]
        public string AudioProfileString
        {
            get
            {
                switch (this.AudioFormat.ToLower())
                {
                    case "ac3":
                    case "dts":
                    case "mpeg audio":
                        {
                            if (this.AudioProfile != null && this.AudioProfile != "")
                                return string.Format("{0} {1}", this.AudioFormat, this.AudioProfile);
                            else
                                return this.AudioFormat;
                        }
                    default:
                        return this.AudioFormat;
                }
            }
        }
          [XmlAttribute()]
        public string AudioCombinedString
        {
            get
            {
                return string.Format("{0} {1}", this.AudioProfileString, this.AudioChannelString);
            }
        }
        #endregion

        #region Properties General
          [XmlAttribute()]
        public string SubtitleString
        {
            get
            {
                return Subtitles;
            }
        }
        #endregion
    }
}
