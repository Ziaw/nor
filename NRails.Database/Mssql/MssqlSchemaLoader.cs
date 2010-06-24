using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Sql;
using BLToolkit.Data.Sql.SqlProvider;
using NRails.Database.Mssql.SqlServer.Schema;
using NRails.Database.Schema;
using System.Linq;

namespace NRails.Database.Mssql
{
	internal static class MssqlSchemaLoader
	{
		public static DBSchema LoadSchema(string constr)
		{
			var csb = new SqlConnectionStringBuilder(constr);
			using (var con = new SqlConnection(csb.ConnectionString))
			{
				con.Open();

				var dbsc = new DBSchema
				{
					Name = csb.InitialCatalog,
					Tables = GetTables(con)
				};

				foreach (var table in dbsc.Tables)
				{
					table.Keys = GetKeys(con, table).ToArray();
					table.Indexes = GetIndexes(con, table).ToArray();
				}

				return dbsc;
			}
		}

		#region Private Methods
		private static LinkRule GetDbsmRule(string rule)
		{
			switch (rule)
			{
				case "CASCADE":
					return LinkRule.Cascade;
				case "SET NULL":
					return LinkRule.SetNull;
				case "SET DEFAULT":
					return LinkRule.SetDefault;
				default:
					return LinkRule.None;
			}
		}

		private static List<TableSchema> GetTables(SqlConnection con)
		{
			var tables = new List<TableSchema>();
			string[] restrict4 = {null, null, null, "TABLE"};

            var dtTables = SqlSchemaFactory.GetSchema(con, "Tables", restrict4);
			for (var i = 0; i < dtTables.Rows.Count; i++)
			{
				var tRow = dtTables.Rows[i];
				TableSchema eTable = GetTable(con, tRow);

			    tables.Add(eTable);
			}
			return tables;
		}

	    private static TableSchema GetTable(SqlConnection con, DataRow tRow)
	    {
	        var eTable = new TableSchema {Name = tRow["TABLE_NAME"].ToString()};
	        // Columns
	        string[] restrict3 = { null, null, null };
	        restrict3[0] = null;
	        restrict3[1] = null;
	        restrict3[2] = eTable.Name;

	        var dtShema = SqlSchemaFactory.GetSchema(con, "Columns", restrict3);
	        if (dtShema.Rows.Count > 0)
	        {
	            eTable.Columns = new TableColumnSchema[dtShema.Rows.Count];

	            for (var j = 0; j < dtShema.Rows.Count; j++)
	            {
	                var cRow = dtShema.Rows[j];

	                var eColumn = new TableColumnSchema
	                                  {
	                                      Name = cRow["COLUMN_NAME"].ToString(),
	                                      Type = ColumnSchemaToSqlDataType(cRow),
	                                      Nullable = Convert.ToBoolean(cRow["IS_NULLABLE"], CultureInfo.InvariantCulture),
	                                      DefaultValue = cRow["COLUMN_DEFAULT"].ToString(),
	                                      Increment = Convert.ToInt32(cRow["IDENTITY_INCREMENT"], CultureInfo.InvariantCulture),
	                                      Seed = Convert.ToInt32(cRow["IDENTITY_SEED"], CultureInfo.InvariantCulture),
	                                      AutoIncrement = Convert.ToBoolean(cRow["IS_IDENTITY"], CultureInfo.InvariantCulture),
	                                  };
	                eColumn.DefaultValue = string.IsNullOrEmpty(eColumn.DefaultValue)
	                                           ? null
	                                           : UnBracket.ParseUnBracket(eColumn.DefaultValue);

	                eTable.Columns[j] = eColumn;
	            }
	        }
	        return eTable;
	    }

	    private static SqlDataType ColumnSchemaToSqlDataType(DataRow cRow)
	    {
            var size = Convert.ToInt32(cRow["COLUMN_SIZE"], CultureInfo.InvariantCulture);
            var decimalPrecision = Convert.ToInt32(cRow["NUMERIC_PRECISION"], CultureInfo.InvariantCulture);
            var decimalScale = Convert.ToInt32(cRow["NUMERIC_SCALE"], CultureInfo.InvariantCulture);
            var type = TypeSqlToDbsm(cRow["COLUMN_DATA_TYPE"].ToString(), size, decimalPrecision, decimalScale);

	        return type;
        }

	    private static List<KeySchema> GetKeys(SqlConnection con, SchemaNamedElement eTable)
		{
			var keys = new List<KeySchema>();
			var aHash = new Dictionary<string, bool>();

			string[] restrict3 = {null, null, null};
			string[] restrict4 = {null, null, null, null};

			#region Primary keys
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
            var dtShema = SqlSchemaFactory.GetSchema(con, "PrimaryKeys", restrict3);
			aHash.Clear();
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				var cName = cRow["CONSTRAINT_NAME"].ToString();
				if (aHash.ContainsKey(cName))
					continue;
				aHash.Add(cName, true);

				var eKey = new KeySchema
				{
					KeyType = ConstraintType.KeyPrimary,
					Name = cName,
					Clustered = Convert.ToBoolean(cRow["IS_CLUSTERED"], CultureInfo.InvariantCulture)
				};

				var columns = new StringBuilder();

				var dtv = dtShema.DefaultView;
				dtv.RowFilter = string.Format("CONSTRAINT_NAME = '{0}'", cName);
				dtv.Sort = "ORDINAL_POSITION ASC";
				for (var y = 0; y < dtv.Count; y++)
				{
					columns.Append(dtv[y]["COLUMN_NAME"]);
					columns.Append(y == dtv.Count - 1 ? string.Empty : ", ");
				}

				eKey.Columns = columns.ToString();
				keys.Add(eKey);
			}
			#endregion

			#region Foreign keys
			restrict4[0] = null;
			restrict4[1] = null;
			restrict4[2] = eTable.Name;
			restrict4[3] = null;
            dtShema = SqlSchemaFactory.GetSchema(con, "ForeignKeys", restrict4);
			aHash.Clear();
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				if (Convert.ToBoolean(cRow["IS_DISABLED"], CultureInfo.InvariantCulture))
					continue;
				var cName = cRow["CONSTRAINT_NAME"].ToString();
				if (aHash.ContainsKey(cName))
					continue;
				aHash.Add(cName, true);

				var eKey = new KeySchema
				{
					KeyType = ConstraintType.KeyForeign,
					Name = cName,
					RelTable = cRow["PK_TABLE_NAME"].ToString(),
					UpdateRule = GetDbsmRule(cRow["UPDATE_RULE"].ToString()),
					DeleteRule = GetDbsmRule(cRow["DELETE_RULE"].ToString())
				};

				var fcolumns = new StringBuilder();
				var rcolumns = new StringBuilder();

				var dtv = dtShema.DefaultView;
				dtv.RowFilter = string.Format("CONSTRAINT_NAME = '{0}'", cName);
				dtv.Sort = "ORDINAL_POSITION ASC";
				for (var y = 0; y < dtv.Count; y++)
				{
					fcolumns.Append(dtv[y]["FK_COLUMN_NAME"]);
					fcolumns.Append(y == dtv.Count - 1 ? string.Empty : ", ");

					rcolumns.Append(dtv[y]["PK_COLUMN_NAME"]);
					rcolumns.Append(y == dtv.Count - 1 ? string.Empty : ", ");
				}

				eKey.Columns = fcolumns.ToString();
				eKey.RelColumns = rcolumns.ToString();
				keys.Add(eKey);
			}
			#endregion

			#region Checks
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
            dtShema = SqlSchemaFactory.GetSchema(con, "CheckConstraints", restrict3);
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var row = dtShema.Rows[x];

				var eKey = new KeySchema
				{
					KeyType = ConstraintType.Check,
					Name = row["CONSTRAINT_NAME"].ToString(),
					Source = row["SOURCE"].ToString()
				};

				keys.Add(eKey);
			}
			#endregion

			#region Unique
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
            dtShema = SqlSchemaFactory.GetSchema(con, "UniqueKeys", restrict3);
			aHash.Clear();
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				var cName = cRow["CONSTRAINT_NAME"].ToString();
				if (aHash.ContainsKey(cName))
					continue;
				var eKey = new KeySchema();
				aHash.Add(cName, true);
				eKey.KeyType = ConstraintType.Unique;
				eKey.Name = cName;

				var columns = new StringBuilder();

				var dtv = dtShema.DefaultView;
				dtv.RowFilter = "CONSTRAINT_NAME = '" + cName + "'";
				dtv.Sort = "ORDINAL_POSITION ASC";
				for (var y = 0; y < dtv.Count; y++)
				{
					columns.Append(dtv[y]["COLUMN_NAME"]);
					columns.Append(y == dtv.Count - 1 ? string.Empty : ", ");
				}

				eKey.Columns = columns.ToString();
				keys.Add(eKey);
			}
			#endregion

			#region Default constraints
			restrict3[0] = null;
			restrict3[1] = null;
			restrict3[2] = eTable.Name;
            dtShema = SqlSchemaFactory.GetSchema(con, "DefaultConstraints", restrict3);
			for (var x = 0; x < dtShema.Rows.Count; x++)
			{
				var cRow = dtShema.Rows[x];
				var eKey = new KeySchema
				{
					KeyType = ConstraintType.Default,
					Name = cRow["CONSTRAINT_NAME"].ToString(),
					Columns = cRow["COLUMN_NAME"].ToString(),
					Source = UnBracket.ParseUnBracket(cRow["SOURCE"].ToString())
				};
				keys.Add(eKey);
			}
			#endregion

			return keys;
		}

		private static List<IndexSchema> GetIndexes(SqlConnection con, TableSchema eTable)
		{
			var indexes = new List<IndexSchema>();
			var aHash = new Dictionary<string, bool>();
			string[] restrict4 = {null, null, null, null};

			// Indexes
			restrict4[0] = null;
			restrict4[1] = null;
			restrict4[2] = eTable.Name;
			restrict4[3] = null;
            var dtShema = SqlSchemaFactory.GetSchema(con, "Indexes", restrict4);
			for (var i = 0; i < dtShema.Rows.Count; i++)
			{
				var row = dtShema.Rows[i];
				if (Convert.ToBoolean(row["IS_STATISTICS"], CultureInfo.InvariantCulture) ||
				    Convert.ToBoolean(row["IS_AUTOSTATISTICS"], CultureInfo.InvariantCulture) ||
				    Convert.ToBoolean(row["IS_HYPOTTETICAL"], CultureInfo.InvariantCulture))
					continue;

				var cName = row["INDEX_NAME"].ToString();
				if (eTable.IsKeyExist(cName, ConstraintType.Unique) ||
				    eTable.IsKeyExist(cName, ConstraintType.KeyPrimary) ||
				    eTable.IsKeyExist(cName, ConstraintType.KeyForeign))
					continue;

				if (aHash.ContainsKey(cName))
					continue;
				var eIndex = new IndexSchema();
				aHash.Add(cName, true);
				eIndex.Name = cName;
				eIndex.Unique = Convert.ToBoolean(row["IS_UNIQUE"], CultureInfo.InvariantCulture);
				eIndex.Clustered = Convert.ToBoolean(row["IS_CLUSTERED"], CultureInfo.InvariantCulture);
				//eIndex.isActive = !Convert.ToBoolean(cRow["IS_INACTIVE"], CultureInfo.InvariantCulture);

				var dtv = dtShema.DefaultView;
				dtv.RowFilter = string.Format("INDEX_NAME = '{0}'", cName);
				dtv.Sort = "COLUMN_ORDINAL_POSITION ASC";

				var columns = "";
				for (var y = 0; y < dtv.Count; y++)
					columns += (dtv[y]["COLUMN_NAME"] +
					            (Convert.ToBoolean(dtv[y]["IS_DESCENDING"], CultureInfo.InvariantCulture) ? " DESC" : "") +
					            ", ");
				columns = columns.Remove(columns.Length - 2, 2);
				eIndex.Columns = columns;
				indexes.Add(eIndex);
			}
			return indexes;
		}
		#endregion

		#region Преобразование типов MsSql Server <-> Dbsm.Types

        static SqlDataType TypeSqlToDbsm(string typeName, int size, int decimalPrecision, int decimalScale)
        {
			switch (typeName.ToUpper(CultureInfo.CurrentCulture))
			{
				case "BIT":
                    return SqlDataType.DbBit;
				case "BIGINT":
                    return SqlDataType.DbBigInt;
				case "BINARY":
                    return new SqlDataType(SqlDbType.Binary, size < 0 ? 8000 : size);
                case "CHAR":
                    return new SqlDataType(SqlDbType.Char, size);
				case "CURSOR":
                    return SqlDataType.DbStructured;
				case "DATETIME":
                    return SqlDataType.DbDateTime;
                case "NUMERIC":
                case "DECIMAL":
					return new SqlDataType(SqlDbType.Decimal, decimalPrecision, decimalScale);
				case "INT":
					return SqlDataType.DbInt;
				case "IMAGE":
                    return new SqlDataType(SqlDbType.Image, size);
				case "FLOAT":
                    return SqlDataType.DbFloat;
                case "MONEY":
                    return new SqlDataType(SqlDbType.Money, decimalPrecision, decimalScale);
				case "NCHAR":
                    return new SqlDataType(SqlDbType.NChar, size);
                case "NVARCHAR":
					return new SqlDataType(SqlDbType.NVarChar, size);
				case "NTEXT":
                    return new SqlDataType(SqlDbType.NText, size);
				case "REAL":
                    return SqlDataType.DbReal;
				case "SMALLMONEY":
                    return new SqlDataType(SqlDbType.SmallMoney, decimalPrecision, decimalScale);
                case "SMALLDATETIME":
                    return SqlDataType.DbSmallDateTime;
				case "SMALLINT":
                    return SqlDataType.DbSmallInt;
                case "SQL_VARIANT":
                    return SqlDataType.DbVariant;
                case "TABLE":
                    return SqlDataType.DbStructured;
				case "TEXT":
                    return new SqlDataType(SqlDbType.Text, size);
                case "TIMESTAMP":
                    return SqlDataType.DbTimestamp;
                case "TINYINT":
                    return SqlDataType.DbTinyInt;
                case "UNIQUEIDENTIFIER":
                    return SqlDataType.Guid;
                case "VARBINARY":
                    return new SqlDataType(SqlDbType.VarBinary, size < 0 ? 8000 : size);
                case "VARCHAR":
                    return new SqlDataType(SqlDbType.VarChar, size);
                case "XML":
                    return SqlDataType.DbXml;
				default:
					throw new ArgumentException("Unsupported data type for " + MssqlDriver.DriverName);
			}
		}

        class SqlTypeBuilder : MsSql2005SqlProvider
        {
            public SqlTypeBuilder(DataProviderBase dataProvider) : base(dataProvider)
            {
            }

            public string BuildType(SqlDataType type)
            {
                // workaround toolkit bug, when money generates with precision and scale, as Money(19,4)
                if (type.DbType == SqlDbType.Money || type.DbType == SqlDbType.SmallMoney)
                {
                    return type.DbType.ToString();
                }
                var sb = new StringBuilder();

                BuildDataType(sb, type);
                return sb.ToString();
            }
        }

		public static string TypeDbsmToSql(TableColumnSchema eColumn)
		{
		    return new SqlTypeBuilder(null).BuildType(eColumn.Type);
		}
		#endregion

	    public static void ReloadTableSchema(SqlConnection conn, DBSchema schema, TableSchema table)
	    {
            string[] restrict4 = { null, null, table.Name, "TABLE" };

            var dtTables = SqlSchemaFactory.GetSchema(conn, "Tables", restrict4);

            if (dtTables.Rows.Count == 0)
            {
                schema.Tables.Remove(table);
                table.Name += "_FANTOM";
                table.Columns = new TableColumnSchema[0];
                table.Keys = new KeySchema[0];
                table.Indexes = new IndexSchema[0];
                return;
            }

	        var freshTable = GetTable(conn, dtTables.Rows[0]);
	    
            table.Columns = freshTable.Columns;
            table.Keys = GetKeys(conn, table).ToArray();
            table.Indexes = GetIndexes(conn, table).ToArray();
        }
	}
}