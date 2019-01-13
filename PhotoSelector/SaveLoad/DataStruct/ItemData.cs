using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSelector.SaveLoad.DataStruct
{
    [Serializable]
    public class ItemData
    {
        [System.Xml.Serialization.XmlAttribute("FilePath")]
        public string FilePath { get; set; }

        [System.Xml.Serialization.XmlAttribute("IsOK")]
        public bool IsOK { get; set; }

        [System.Xml.Serialization.XmlAttribute("IsKeep")]
        public bool IsKeep { get; set; }
    }
}
