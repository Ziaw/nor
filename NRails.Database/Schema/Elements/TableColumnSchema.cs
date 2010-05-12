using System;
using System.ComponentModel;
using System.Data;
using System.Xml.Serialization;
using BLToolkit.Data.Sql;

namespace NRails.Database.Schema
{
    public class TableColumnSchema : SchemaNamedElement, IEquatable<TableColumnSchema>
    {
		private const bool _defaultAutoIncrement = false;
		private const int _defaultDecimalPrecision = 0;
		private const int _defaultDecimalScale = 0;
		private const int _defaultIncrement = 0;
		private const int _defaultSeed = 0;

        public SqlDataType Type { get; set;}

		[XmlAttribute("auto-increment")]
		[DefaultValue(_defaultAutoIncrement)]
		public bool AutoIncrement { get; set; }

		[XmlAttribute("seed")]
		[DefaultValue(_defaultSeed)]
		public int Seed { get; set; }

		[XmlAttribute("increment")]
		[DefaultValue(_defaultIncrement)]
		public int Increment { get; set; }

		[XmlAttribute("nullable")]
		public bool Nullable { get; set; }

        [XmlAttribute("size")]
        public int Size
        {
            get
            {
                return Type.Length;
            }
            set
            {
                if (Type == null)
                    Type = new SqlDataType(SqlDbType.Structured, value);
                else
                    Type = new SqlDataType(Type.DbType, value);
            }
        }

		[XmlAttribute("decimal-scale")]
		[DefaultValue(_defaultDecimalScale)]
		public int DecimalScale
        {
            get
            {
                return Type.Scale;
            }
            set
            {
                if (Type == null)
                    Type = new SqlDataType(SqlDbType.Structured, value, 0);
                else
                    Type = new SqlDataType(Type.DbType, Type.Precision, value);
            }
        }

        [XmlAttribute("decimal-precision")]
        [DefaultValue(_defaultDecimalPrecision)]
        public int DecimalPrecision
        {
            get
            {
                return Type.Scale;
            }
            set
            {
                if (Type == null)
                    Type = new SqlDataType(SqlDbType.Structured, 0, value);
                else
                    Type = new SqlDataType(Type.DbType, Type.Precision, value);
            }
        }

		[XmlAttribute("default-value")]
		public string DefaultValue { get; set; }

		#region Methods

        public bool Equals(TableColumnSchema other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.Type, Type) && other.AutoIncrement.Equals(AutoIncrement) && other.Seed == Seed && other.Increment == Increment && other.Nullable.Equals(Nullable) && other.Size == Size && other.DecimalScale == DecimalScale && other.DecimalPrecision == DecimalPrecision && Equals(other.DefaultValue, DefaultValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as TableColumnSchema);
        }

        public override int GetHashCode()
		{
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ (Type != null ? Type.GetHashCode() : 0);
                result = (result*397) ^ AutoIncrement.GetHashCode();
                result = (result*397) ^ Seed;
                result = (result*397) ^ Increment;
                result = (result*397) ^ Nullable.GetHashCode();
                result = (result*397) ^ Size.GetHashCode();
                result = (result*397) ^ DecimalScale;
                result = (result*397) ^ DecimalPrecision;
                result = (result*397) ^ (DefaultValue != null ? DefaultValue.GetHashCode() : 0);
                return result;
            }
		}

        public TableColumnSchema Clone()
        {
            return (TableColumnSchema) MemberwiseClone();
        }

	    #endregion
	}
}