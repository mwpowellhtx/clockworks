using System;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// Provides some boiler plate disposable functionality.
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        protected Disposable()
        {
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~Disposable()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposed backing field.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Gets whether IsDisposed.
        /// </summary>
        protected bool IsDisposed
        {
            get { return _disposed; }
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            Dispose(true);
            _disposed = true;
        }
    }
}
