using System.ComponentModel;
using System.Xml.Serialization;

namespace NRails.Database.Schema
{
	/// <summary>
	/// ������� ����� � ������.
	/// </summary>
	public class SchemaNamedElement
	{
		[XmlAttribute("name")]
		[Localizable(false)]
		public string Name { get; set; }
	}
}