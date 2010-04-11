using System.Data;

namespace Rsdn.Janus
{
	public interface IDBSchemaDriver
	{
		/// <summary>
		/// ������� ��.
		/// </summary>
		/// <param name="constr">������ �����������</param>
		void CreateDatabase(string constr);

	    /// <summary>
	    /// ������� ����� ���������� �� �������� ����
	    /// </summary>
	    /// <param name="connStr"></param>
	    DBSchema LoadExistingSchema(string connStr);

		/// <summary>
		/// ��������� ����� � ��������� � ������ �� ���������� ������� DDL �������
		/// </summary>
        void CompareDbsc(DBSchema dbsc, string targetConnStr, DataSet schemaDataset);

		/// <summary>
		/// �������� �������� ���� � ��� � ������� ����������.
		/// </summary>
		/// <param name="connStr"></param>
		void Prepare(string connStr);

		/// <summary>
		/// ������ �������������� DDL ������� � ����
		/// </summary>
		/// <param name="path">���� � ����� ��� ������ ������</param>
		void PrepareToSqlFile(string path);

		IDbConnection CreateConnection(string connStr);
		string MakeSelect(TableSchema table, bool orderedByPK);
		string MakeInsert(TableSchema table);
		IDbDataParameter ConvertToDbParameter(TableColumnSchema column, IDbDataParameter parameter);
		void BeginTableLoad(IDbConnection connection, TableSchema table);
		void EndTableLoad(IDbConnection connection, TableSchema table);
	}
}