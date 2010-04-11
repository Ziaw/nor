using System.ComponentModel;
using System.Xml.Serialization;

namespace Rsdn.Janus
{
	/// <summary>
	/// Ёлемент схемы с именем.
	/// </summary>
	public class SchemaNamedElement
	{
		[XmlAttribute("name")]
		[Localizable(false)]
		public string Name { get; set; }
	}
}