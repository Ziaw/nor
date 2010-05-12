using System.Collections.Generic;
using System.Data;
using NRails.Database.Schema;

namespace NRails.Database.Schema
{
	public interface IDBSchemaDriver
	{
		/// <summary>
		/// ������� ��.
		/// </summary>
		/// <param name="constr">������ �����������</param>
		void CreateDatabase(string constr);

        /// <summary>
        /// ��������� ������� ��.
        /// </summary>
        /// <param name="connectionString">������ �����������</param>
        bool DatabaseExists(string connectionString);

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
		void BeginTableLoad(IDbConnection connection, TableSchema table);
		void EndTableLoad(IDbConnection connection, TableSchema table);
	    string MakeDdlTableCreate(TableSchema table, bool withConstraint);
	    void ExecuteDdlCommands(IEnumerable<string> commands, string connStr);
	    string MakeDdlColumnCreate(TableColumnSchema column, TableSchema table);
	    string MakeDdlTableDrop(TableSchema table);
	    string MakeDdlTableRename(TableSchema table, string newName);

	    string MakeDdlColumnAlter(TableColumnSchema mColumn, TableColumnSchema eColumn,
	                                              TableSchema table);

	    string MakeDdlColumnDrop(TableColumnSchema column, TableSchema table);
	    void ReloadTableSchema(IDbConnection conn, DBSchema schema, TableSchema table);
	    string MakeDdlKeyDrop(KeySchema key, TableSchema table);
	}
}