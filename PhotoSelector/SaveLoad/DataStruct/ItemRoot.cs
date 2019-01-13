using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSelector.SaveLoad.DataStruct
{
    [System.Xml.Serialization.XmlRoot("PhotoSelectData")]
    public class ItemRoot
    {
        public List<ItemData> Records = new List<ItemData>();
    }
}
