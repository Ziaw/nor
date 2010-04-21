using System.Xml.Serialization;

namespace NRails.Database.Schema
{
	public class TableSchema : SchemaNamedElement
	{
		[XmlElement("column")]
		public TableColumnSchema[] Columns { get; set; }

		[XmlElement("index")]
		public IndexSchema[] Indexes { get; set; }

		[XmlElement("key")]
		public KeySchema[] Keys { get; set; }

		public TableSchema Clone()
		{
			return new TableSchema
					{
						Name = Name,
						Columns = (TableColumnSchema[]) Columns.Clone(),
						Indexes = (IndexSchema[]) Indexes.Clone(),
						Keys = (KeySchema[]) Keys.Clone()
					};
		}
	}
}