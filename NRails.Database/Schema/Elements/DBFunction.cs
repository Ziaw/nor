using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;

namespace NRails.Database.Schema
{
    public class DBFunction
    {
        private List<DbType> _params = new List<DbType>();

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("params", IsNullable = false)]
        public List<DbType> Params
        {
            get { return _params; }
            set { _params = value; }
        }
    }
}