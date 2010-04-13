using JetBrains.Annotations;

namespace NRails.Database
{
	/// <summary>
	/// ������, �������������� ������ � ���������� � ���������.
	/// </summary>
	public interface IDBDriverManager
	{
		/// <summary>
		/// ���������� ��������� �������� �� ��� �����.
		/// </summary>
		[NotNull]
		IDBDriver GetDriver([NotNull] string driverName);
	}
}