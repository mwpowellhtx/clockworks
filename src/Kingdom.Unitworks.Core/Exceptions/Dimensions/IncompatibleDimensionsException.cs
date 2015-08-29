using System;
using System.Collections.Generic;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public class IncompatibleDimensionsException : Exception
    {
        private readonly List<IQuantity> _quantities = new List<IQuantity>();

        /// <summary>
        /// 
        /// </summary>
        public ICollection<IQuantity> Quantities
        {
            get { return _quantities; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantities"></param>
        public IncompatibleDimensionsException(params IQuantity[] quantities)
        {
            _quantities.AddRange(quantities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="quantities"></param>
        public IncompatibleDimensionsException(string message, params IQuantity[] quantities)
            : base(message)
        {
            _quantities.AddRange(quantities);
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="quantities"></param>
        public IncompatibleDimensionsException(string message, Exception innerException, params IQuantity[] quantities)
            : base(message, innerException)
        {
            _quantities.AddRange(quantities);
        }
    }
}
