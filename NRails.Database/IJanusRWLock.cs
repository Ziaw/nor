using System;

namespace NRails.Database
{
	/// <summary>
	/// Read-write lock.
	/// </summary>
	public interface IJanusRWLock
	{
		IDisposable GetReaderLock();
		IDisposable GetWriterLock();
		IDisposable UpgradeToWriterLock();
	}
}