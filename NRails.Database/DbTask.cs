using System;

namespace NRails.Database
{
	[Flags]
	public enum DBTask
	{
		None = 0x0000,
		Compact = 0x0001,
	}
}