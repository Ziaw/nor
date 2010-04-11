using System;
using System.Threading;
using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Standard <see cref="IJanusRWLock"/> implementation, based on <see cref="ReaderWriterLock"/>.
	/// </summary>
	public class StandardJanusRWLock : IJanusRWLock
	{
        const int millisecondsTimeout = 1000; //
        private readonly ReaderWriterLock _rwLock;

		public StandardJanusRWLock() : this(new ReaderWriterLock())
		{
		}

		public StandardJanusRWLock(ReaderWriterLock rwLock)
		{
			_rwLock = rwLock;
		}

        class DisposableHelper: IDisposable
        {
            private Action disposeAction;

            public DisposableHelper([NotNull]Action disposeAction)
            {
                this.disposeAction = disposeAction;
            }

            public void Dispose()
            {
                disposeAction();
            }
        }

		#region Implementation of IJanusRWLock
		public IDisposable GetReaderLock()
		{
		    _rwLock.AcquireReaderLock(millisecondsTimeout);
			return 
                _rwLock.GetReaderLock();
		}

		public IDisposable GetWriterLock()
		{
			return _rwLock.GetWriterLock();
		}

		public IDisposable UpgradeToWriterLock()
		{
			return _rwLock.UpgradeToWriterLock();
		}
		#endregion
	}
}