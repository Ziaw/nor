using JetBrains.Annotations;

namespace Rsdn.Janus
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

		/// <summary>
		/// ���������� ������ ������������������ ���������.
		/// </summary>
		[NotNull]
		JanusDBDriverInfo[] GetRegisteredDriverInfos();

		[NotNull]
		JanusDBDriverInfo GetDriverInfo([NotNull] string driverName);
	}
}