using System;
using System.Threading;
using System.Threading.Tasks;

namespace Counsel.Core
{
	public class AsyncLock : IDisposable
	{
		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

		private bool _disposed;

		public async Task<IDisposable> LockAsync()
		{
			await _semaphore.WaitAsync();
			return new LockToken(() => _semaphore.Release());
		}

		#region IDisposable Support

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_semaphore.Dispose();
				}

				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		private class LockToken : IDisposable
		{
			private bool _disposed;

			private readonly Action _onDispose;

			public LockToken(Action onDispose)
			{
				_onDispose = onDispose;
			}

			protected virtual void Dispose(bool disposing)
			{
				if (!_disposed)
				{
					if (disposing)
					{
						_onDispose();
					}

					_disposed = true;
				}
			}

			public void Dispose()
			{
				Dispose(true);
			}
		}
	}
}
