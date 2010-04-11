using System.Data.Common;

using BLToolkit.Data.DataProvider;

namespace Rsdn.Janus
{
	/// <summary>
	/// ��������� �������� ��.
	/// </summary>
	public interface IDBDriver
	{
		/// <summary>
		/// ��������� ������ �����������.
		/// </summary>
		/// <param name="constr">������ �����������</param>
		bool CheckConnectionString(string constr);

		/// <summary>
		/// �������� ������� �����.
		/// </summary>
		IDBSchemaDriver CreateSchemaDriver();

		/// <summary>
		/// ������� ������-����������� ������� ����������.
		/// </summary>
		/// <returns>������-����������� ������� ����������</returns>
		DbConnectionStringBuilder CreateConnectionString();

		/// <summary>
		/// ������� ������-����������� ������� �����������, ������������������ ���.
		/// </summary>
		/// <param name="constr">��������� �������� ������ �����������</param>
		/// <returns>������-����������� ������� �����������</returns>
		DbConnectionStringBuilder CreateConnectionString(string constr);


		/// <summary>
		/// ������� ��������� ��� BLToolkit.
		/// </summary>
		DataProviderBase CreateDataProvider();

		/// <summary>
		/// ���������� ������ ����� �����������.
		/// </summary>
		string PreprocessQueryText(string text);

		/// <summary>
		/// ������ �� ��������� SQL.
		/// </summary>
		ISqlFormatter Formatter { get; }
	}
}