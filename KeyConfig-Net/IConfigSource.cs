using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyConfig
{
    /// <summary>
    /// Conveys a configuration source, of which configuration settings may be derived from.
    /// </summary>
    public interface IConfigSource
    {
        /// <summary>
        /// Indicates whether the source can have configuration settings written to as well as read.
        /// </summary>
        bool CanSet { get; }

        /// <summary>
        /// Indicates whether a specified config key type can be handled as in stored or retrieved as a config value.
        /// </summary>
        /// <param name="objTyp">The type to check.</param>
        /// <returns>True if the type is allowed, false otherwise.</returns>
        bool GetCanHandle(Type objTyp);

        /// <summary>
        /// Sets a value to the configuration source. Is not called ever if the source indicates its read only.
        /// </summary>
        /// <param name="key">The key to the configuration value.</param>
        /// <param name="value">The value that is to be written to the configuration key.</param>
        /// <param name="instanceType">The object type of which configuration values are being mapped to.</param>
        /// <param name="valueType">The value type.</param>
        void SetValue(string key, object value, Type instanceType, Type valueType);

        /// <summary>
        /// Retrieves a value from the configuration source.
        /// </summary>
        /// <param name="key">The key to the configuration value.</param>
        /// <param name="instanceType">The object type of which configuration values are being mapped to.</param>
        /// <param name="valueType">The value type of what is expected to be returned.</param>
        /// <returns>The object retrieved at the specified key, assuming the valueType. Will be null if the value was not found at the specified key.</returns>
        object GetValue(string key, Type instanceType, Type valueType);

    }
}
