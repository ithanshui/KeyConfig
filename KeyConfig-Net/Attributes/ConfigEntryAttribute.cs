using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyConfig
{
    /// <summary>
    /// Indicates a specified property refers to a configuration setting for Key Config.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ConfigKeyAttribute : Attribute
    {
        /// <summary>
        /// Indicates whether the value is required. If the value is not supplied either while getting or setting configuration values, an exception is thrown.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// The configuration key this property refers to. If left null, the name of the property itself is used.
        /// </summary>
        public string KeyName { get; set; }
    }
}
