using System.ComponentModel;
using System.Xml.Serialization;

namespace NRails.Database.Schema
{
	/// <summary>
	/// Ёлемент схемы с именем.
	/// </summary>
	public class SchemaNamedElement
	{
		[XmlAttribute("name")]
		[Localizable(false)]
		public string Name { get; set; }

	    public bool Equals(SchemaNamedElement other)
	    {
	        if (ReferenceEquals(null, other)) return false;
	        if (ReferenceEquals(this, other)) return true;
	        return Equals(other.Name, Name);
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != typeof (SchemaNamedElement)) return false;
	        return Equals((SchemaNamedElement) obj);
	    }

	    public override int GetHashCode()
	    {
	        return (Name != null ? Name.GetHashCode() : 0);
	    }
	}
}