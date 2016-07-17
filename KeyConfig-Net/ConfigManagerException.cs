using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyConfig
{
    /// <summary>
    /// An exception that represents an error which occurred in the key config process.
    /// </summary>
    public class ConfigManagerException : Exception
    {
        /// <summary>
        /// Creates a new instance of ConfigManagerException.
        /// </summary>
        public ConfigManagerException() { }

        /// <summary>
        /// Creates a new instance of ConfigManagerException with a specific message.
        /// </summary>
        /// <param name="message">The messsage to use.</param>
        public ConfigManagerException(string message) : base(message) { }

        /// <summary>
        /// Creates a new instance of ConfigManagerException with a message and inner exception.
        /// </summary>
        /// <param name="message">The messsage to use.</param>
        /// <param name="inner">The inner exception.</param>
        public ConfigManagerException(string message, Exception inner) : base(message, inner) { }

    }
}
