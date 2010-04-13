using System;

namespace NRails.Database.Schema
{
	[Flags]
	public enum IndexClearType
	{
		None = 0,
		Desc = 1,
		Asc = 2,
		All = 3
	}
}