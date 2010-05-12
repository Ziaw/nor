using System.Collections.Generic;
using System.Data;
using NRails.Database.Schema;

namespace NRails.Database.Schema
{
	public interface IDBSchemaDriver
	{
		/// <summary>
		/// Создать БД.
		/// </summary>
		/// <param name="constr">строка подключения</param>
		void CreateDatabase(string constr);

        /// <summary>
        /// Проверить наличие БД.
        /// </summary>
        /// <param name="connectionString">строка подключения</param>
        bool DatabaseExists(string connectionString);

        /// <summary>
	    /// Создать схему метаданных из исходной базы
	    /// </summary>
	    /// <param name="connStr"></param>
	    DBSchema LoadExistingSchema(string connStr);

		/// <summary>
		/// Сравнение схемы с эталонной и выдача во внутреннюю таблицу DDL комманд
		/// </summary>
        void CompareDbsc(DBSchema dbsc, string targetConnStr, DataSet schemaDataset);

		/// <summary>
		/// Привести исходную базу к той с которой сравнивали.
		/// </summary>
		/// <param name="connStr"></param>
		void Prepare(string connStr);

		/// <summary>
		/// Запись сгенерированых DDL комманд в файл
		/// </summary>
		/// <param name="path">путь к файлу для записи команд</param>
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