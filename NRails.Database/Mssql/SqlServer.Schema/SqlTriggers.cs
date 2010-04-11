using System;
using System.Data;
using System.Globalization;

namespace Rsdn.Janus
{
	internal class SqlTriggers : SqlDbSchema
	{
	    public SqlTriggers()
	    {
	    }

	    protected override SqlClause GetCommandText(object[] restrictions)
		{
			var sql = new SqlClause();

			var col = _sqlClauses.Select("CollectionName = 'Triggers' and Version=" + _version.ver1);

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
					sql.AppendWhereFormat(CultureInfo.CurrentCulture, col[0]["Where3"].ToString(), index++);

				// TRIGGER_NAME
				if (restrictions.Length >= 4 && restrictions[3] != null)
					sql.AppendWhereFormat(CultureInfo.CurrentCulture, col[0]["Where4"].ToString(), index);
			}

			sql.AppendOrder(col[0]["Order1"].ToString());

			return sql;
		}

		protected override DataTable ProcessResult(DataTable schema)
		{
			schema.BeginLoadData();

			foreach (DataRow row in schema.Rows)
			{
				// ToBoolean
				foreach (DataColumn col in schema.Columns)
					if (col.Caption.StartsWith("IS_"))
						row[col.Caption] =
							row[col.Caption] != DBNull.Value &&
								Convert.ToInt32(row[col.Caption], CultureInfo.InvariantCulture) != 0;

				if (Convert.ToInt32(row["HELPER_TID"], CultureInfo.InvariantCulture) != 0)
					row["SOURCE"] =
						HelperGetObjectDefinition(Convert.ToInt32(row["HELPER_TID"], CultureInfo.InvariantCulture));
			}

			schema.EndLoadData();
			schema.AcceptChanges();

			return schema;
		}
	}
}