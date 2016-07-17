using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace KeyConfig.ConfigSources
{
    /// <summary>
    /// Defines a source to the app settings config section of an app or web config.
    /// </summary>
    public class AppSettingsSource : IConfigSource
    {
        /// <summary>
        /// Indicates whether the source can have configuration settings written to as well as read.
        /// </summary>
        public bool CanSet
        {
            get { return true; }
        }


        /// <summary>
        /// Indicates whether a specified config key type can be handled as in stored or retrieved as a config value.
        /// </summary>
        /// <param name="objTyp">The type to check.</param>
        /// <returns>True if the type is allowed, false otherwise.</returns>
        public bool GetCanHandle(Type objTyp)
        {
            if (objTyp == typeof(string) || objTyp == typeof(decimal) || objTyp == typeof(DateTime) 
                || objTyp == typeof(TimeSpan) || objTyp.IsPrimitive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Sets a value to the app settings source.
        /// </summary>
        /// <param name="key">The key to the configuration value.</param>
        /// <param name="value">The value that is to be written to the configuration key.</param>
        /// <param name="instanceType">The object type of which configuration values are being mapped to.</param>
        /// <param name="valueType">The value type.</param>
        public void SetValue(string key, object value, Type instanceType, Type valueType)
        {
            updateSetting(key, value.ToString());
        }

        /// <summary>
        /// Retrieves a value from the configuration source.
        /// </summary>
        /// <param name="key">The key to the configuration value.</param>
        /// <param name="instanceType">The object type of which configuration values are being mapped to.</param>
        /// <param name="valueType">The value type of what is expected to be returned.</param>
        /// <returns>The object retrieved at the specified key, assuming the value type.</returns>
        public object GetValue(string key, Type instanceType, Type valueType)
        {
            string value = ConfigurationManager.AppSettings[key];

            if (value == null)
            {
                return null;
            }

            if (valueType == typeof(string))
            {
                return value;
            }

            if (valueType == typeof(DateTime))
            {
                return DateTime.Parse(value);
            }

            if (valueType == typeof(TimeSpan))
            {
                return TimeSpan.Parse(value);
            }

            if (valueType == typeof(bool))
            {
                return bool.Parse(value);
            }

            if (valueType == typeof(int))
            {
                return int.Parse(value);
            }

            if (valueType == typeof(short))
            {
                return short.Parse(value);
            }

            if (valueType == typeof(long))
            {
                return long.Parse(value);
            }

            if (valueType == typeof(uint))
            {
                return uint.Parse(value);
            }

            if (valueType == typeof(ushort))
            {
                return ushort.Parse(value);
            }

            if (valueType == typeof(ulong))
            {
                return ulong.Parse(value);
            }

            if (valueType == typeof(Single))
            {
                return Single.Parse(value);
            }

            if (valueType == typeof(char))
            {
                return char.Parse(value);
            }

            if (valueType == typeof(double))
            {
                return char.Parse(value);
            }

            if (valueType == typeof(decimal))
            {
                return decimal.Parse(value);
            }


            throw new NotSupportedException("Type not supported for configuration source.");

        }

        private void updateSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
