using System.Data.Common;

using BLToolkit.Data.DataProvider;
using NRails.Database.Schema;

namespace NRails.Database
{
	/// <summary>
	/// Интерфейс драйвера БД.
	/// </summary>
	public interface IDBDriver
	{
		/// <summary>
		/// Проверить строку подключения.
		/// </summary>
		/// <param name="constr">строка подключения</param>
		bool CheckConnectionString(string constr);

		/// <summary>
		/// Получить драйвер схемы.
		/// </summary>
		IDBSchemaDriver CreateSchemaDriver();

		/// <summary>
		/// Создать объект-манипулятор строкой соединения.
		/// </summary>
		/// <returns>объект-манипулятор строкой соединения</returns>
		DbConnectionStringBuilder CreateConnectionString();

		/// <summary>
		/// Создать объект-манипулятор строкой подключения, проинициализировав его.
		/// </summary>
		/// <param name="constr">начальное значение строки подключения</param>
		/// <returns>объект-манипулятор строкой подключения</returns>
		DbConnectionStringBuilder CreateConnectionString(string constr);


		/// <summary>
		/// Создать провайдер для BLToolkit.
		/// </summary>
		DataProviderBase CreateDataProvider();

		/// <summary>
		/// Обработать запрос перед выполнением.
		/// </summary>
		string PreprocessQueryText(string text);
	}
}