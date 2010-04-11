﻿using BLToolkit.Data;

namespace Rsdn.Janus
{
	public interface IJanusDatabaseManager
	{
		string GetCurrentDriverName();
		IDBDriver GetCurrentDriver();
		string GetCurrentConnectionString();

		/// <summary>
		/// Возвращает лок для работы с БД. Необходим для движков, которые
		/// не поддерживают нормальный параллельный доступ (Jet, Sqlite).
		/// </summary>
		IJanusRWLock GetLock();

		DbManager CreateDBManager();
	}
}
