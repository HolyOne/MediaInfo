using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Security;
using System.Xml.Serialization;

namespace WindowsFormsApplication2
{
     [XmlRoot()]
   public  class SaveFile
    {
        [XmlAttribute()]
       public DateTime GenerateTime{ get; set; }
       [XmlAttribute()]
       public string  PCFingerPrint { get; set; }
        [XmlArray()]
       public List<MediaBrowser.Library.Entities.Media> Medias { get; set; }
       public bool forThisPC()
       {
       return (PCFingerPrint==FingerPrint.Value());
       
       }
      public SaveFile()
      {
     //     PCFingerPrint = FingerPrint.Value();
          GenerateTime = DateTime.Now;
Medias = new List<MediaBrowser.Library.Entities.Media>();

      }

    }
}
