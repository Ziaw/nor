namespace NRails.Database.Schema
{
	/// <summary>
	/// ������ �������� � �������.
	/// </summary>
	public enum IndexNullAllowance
	{
		/// <summary>
		/// ������ �������� ���������.
		/// </summary>
		Allow,

		/// <summary>
		/// ������ �������� ���������.
		/// </summary>
		Disallow,

		/// <summary>
		/// ������ �������� ������������.
		/// </summary>
		Ignore
	}
}