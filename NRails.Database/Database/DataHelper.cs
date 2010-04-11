using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

using BLToolkit.Data;
using BLToolkit.Data.Linq;

using JetBrains.Annotations;
using Rsdn.Janus.DataModel;

namespace Rsdn.Janus
{
	public static class DataHelper
	{
		public static IDbTransaction BeginTx(
			[NotNull] this DbManager db,
			IsolationLevel isolationLevel)
		{
			if (db == null) throw new ArgumentNullException("db");
			return db.BeginTransaction(isolationLevel).Transaction;
		}

		public static IDbTransaction BeginTx([NotNull] this DbManager db)
		{
			if (db == null) throw new ArgumentNullException("db");
			return db.BeginTransaction().Transaction;
		}

		/// <summary>
		/// Получить перечислитель заданной колонки таблицы.
		/// </summary>
		/// <typeparam name="T">тип к которому привести значение колонки</typeparam>
		/// <param name="table">таблица</param>
		/// <param name="name">имя колонки для получения</param>
		/// <returns>перечислитель колонки таблицы</returns>
		public static IEnumerable<T> GetTableColumn<T>(this DataTable table, string name)
		{
			return
				table
					.Rows
					.Cast<DataRow>()
					.Select(row => (T)row[name]);
		}

		[NotNull]
		public static IQueryable<T> GetTable<T>(
			[NotNull] this DbManager dbMgr,
			[CanBeNull] Expression<Func<T, bool>> predicate)
		{
			if (dbMgr == null) throw new ArgumentNullException("dbMgr");
			var tbl = new Table<T>(dbMgr);
			return
				predicate == null
					? tbl
					: tbl.Where(predicate);
		}

		public static Table<IVariable> Vars([NotNull] this DbManager dbMgr)
		{
			if (dbMgr == null) throw new ArgumentNullException("dbMgr");
			return dbMgr.GetTable<IVariable>();
		}

		public static IQueryable<IVariable> Vars(
			[NotNull] this DbManager dbMgr,
			[CanBeNull] Expression<Func<IVariable, bool>> predicate)
		{
			return GetTable(dbMgr, predicate);
		}

		public static void Execute(
			[NotNull] this DbManager db,
			[NotNull] string sql,
			params IDbDataParameter[] parameters)
		{
			if (db == null) throw new ArgumentNullException("db");
			if (sql == null) throw new ArgumentNullException("sql");
			db.SetCommand(sql, parameters).ExecuteNonQuery();
		}
	}
}