using System;
using System.Data.Common;
using System.Data.SqlClient;

using BLToolkit.Data.DataProvider;

namespace Rsdn.Janus.Mssql
{
	[JanusDBDriver(
		DriverName,
		"Rsdn.Janus.Mssql.Resources",
		"DriverDisplayName",
		"DriverDescription")]
	public class MssqlDriver : IDBDriver
	{
		public const string DriverName = "MSSql";
		private readonly MssqlSqlFormatter _sqlFormatter = new MssqlSqlFormatter();

		public MssqlDriver()
		{
		}

		#region IDBDriver Members
		public bool CheckConnectionString(string constr)
		{
			try
			{
				var csbCheck = new SqlConnectionStringBuilder(constr);

				using (var con = new SqlConnection(csbCheck.ConnectionString))
					con.Open();
			}
			catch
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// �������� ������� �����.
		/// </summary>
		/// <returns></returns>
		public IDBSchemaDriver CreateSchemaDriver()
		{
			return new MssqlSchemaDriver();
		}

		public DbConnectionStringBuilder CreateConnectionString()
		{
			return new SqlConnectionStringBuilder();
		}

		public DbConnectionStringBuilder CreateConnectionString(string constr)
		{
			return new SqlConnectionStringBuilder(constr);
		}

		/// <summary>
		/// ������� ��������� ��� BLToolkit.
		/// </summary>
		public DataProviderBase CreateDataProvider()
		{
			return new SqlDataProvider();
		}

		/// <summary>
		/// ���������� ������ ����� �����������.
		/// </summary>
		public string PreprocessQueryText(string text)
		{
			return text;
		}

		/// <summary>
		/// ������ �� ��������� SQL.
		/// </summary>
		public ISqlFormatter Formatter
		{
			get { return _sqlFormatter; }
		}
		#endregion
	}
}