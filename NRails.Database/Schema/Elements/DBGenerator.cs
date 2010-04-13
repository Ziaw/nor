using System.Xml.Serialization;

namespace NRails.Database.Schema
{
	public class DBGenerator : SchemaNamedElement
	{
		[XmlAttribute("start-value")]
		public int StartValue { get; set; }
	}
}