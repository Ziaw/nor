using System;
using System.Data;
using System.Globalization;

namespace NRails.Database.Mssql.SqlServer.Schema
{
	internal class SqlPrimaryKeys : SqlDbSchema
	{
	    public SqlPrimaryKeys()
	    {
	    }

	    protected override SqlClause GetCommandText(object[] restrictions)
		{
			var sql = new SqlClause();

			var col = _sqlClauses.Select("CollectionName = 'PrimaryKeys' and Version=" + _version.ver1);

			sql.Select = col[0]["SelectMain"].ToString();

			sql.AppendWhere(col[0]["Where1"].ToString());

			if (restrictions != null)
			{
				var index = 0;

				// TABLE_CATALOG
				if (restrictions.Length >= 1 && restrictions[0] != null)
				{}

				// TABLE_SCHEMA
				if (restrictions.Length >= 2 && restrictions[1] != null)
					sql.AppendWhereFormat(CultureInfo.CurrentCulture, col[0]["Where2"].ToString(), index++);

				// TABLE_NAME
				if (restrictions.Length >= 3 && restrictions[2] != null)
					sql.AppendWhereFormat(CultureInfo.CurrentCulture, col[0]["Where3"].ToString(), index);
			}

			sql.AppendOrder(col[0]["Order1"].ToString());

			return sql;
		}

		protected override DataTable ProcessResult(DataTable schema)
		{
			schema.BeginLoadData();

			foreach (DataRow row in schema.Rows)
				foreach (DataColumn col in schema.Columns)
					if (col.Caption.StartsWith("IS_"))
						row[col.Caption] =
							row[col.Caption] != DBNull.Value &&
								Convert.ToInt32(row[col.Caption], CultureInfo.InvariantCulture) != 0;

			schema.EndLoadData();
			schema.AcceptChanges();

			return schema;
		}
	}
}