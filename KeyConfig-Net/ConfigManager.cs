using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KeyConfig
{

    /// <summary>
    /// Facilitates access to configuration settings.
    /// </summary>
    /// <typeparam name="T">Class type of which to map configuration settings to and from.</typeparam>
    public static class ConfigManager<T> where T : class
    {
        const BindingFlags PROP_BINDINGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        /// <summary>
        /// Returns an indication of whether all values that are marked as required are fulfilled.
        /// </summary>
        /// <param name="source">The configuration source.</param>
        /// <param name="instance">The instance to check for required values.</param>
        /// <exception cref="ConfigManagerException">Thrown when a value couldn't be retrieved.</exception>
        /// <returns>True if all required values are found; else false.</returns>
        public static bool CheckRequired(IConfigSource source)
        {
            foreach (var key in getConfigKeys(source, typeof(T)))
            {
                try
                {
                    var value = source.GetValue(getKeyName(key), typeof(T), key.PropertyType);

                    if (getIsRequired(key))
                    {
                        if (!key.PropertyType.IsValueType)
                        {
                            if (value == null)
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new ConfigManagerException(string.Format("Failed to get config value to instance for key '{0}'", key.Name), e);
                }
            }

            return true;
        }

        /// <summary>
        /// Creates an instance of T and then populates configuration properties from the specified source.
        /// </summary>
        /// <param name="source">The configuration source.</param>
        /// <exception cref="ConfigManagerException">Thrown when a value couldn't be retrieved, a required field couldn't be fulfilled, or an instance of T couldn't be created.</exception>
        /// <returns>A new instance of T, of which has configuration properties populated.</returns>
        public static T GetConfig(IConfigSource source)
        {
            T instance = null;

            try
            {
                instance = (T)Activator.CreateInstance(typeof(T));
            }
            catch (Exception e)
            {
                throw new ConfigManagerException(string.Format("Failed to create an instance of '{0}'. See inner exception.", typeof(T).Name), e);
            }

            FulfillConfig(source, instance);

            return instance;
        }

        /// <summary>
        /// Populates an existing instance of T with configuration settings from the specified source.
        /// </summary>
        /// <param name="source">The configuration source.</param>
        /// <param name="instance">Existing instance of T to populate with configuration settings.</param>
        /// <exception cref="ConfigManagerException">Thrown when a value couldn't be retrieved.</exception>
        public static void FulfillConfig(IConfigSource source, T instance)
        {
            foreach (var key in getConfigKeys(source, typeof(T)))
            {
                try
                {
                    var value = source.GetValue(getKeyName(key), typeof(T), key.PropertyType);

                    if (getIsRequired(key))
                    { 
                        if (value == null)
                        {
                            throw new ConfigManagerException(string.Format("Failed to get config value for key '{0}'. Value is required but was not specified.", key.Name));
                        }                   
                    }               

                    if (value == null)
                    {
                        value = getDefaultValue(key);
                    }

                    key.SetValue(instance, value, null);
                }
                catch (Exception e)
                {
                    throw new ConfigManagerException(string.Format("Failed to get config value to instance for key '{0}' See inner exception.", key.Name), e);
                }
            }
        }

        /// <summary>
        /// Saves all configuration settings from the instance of T to the specified source.
        /// </summary>
        /// <param name="source">The configuration source.</param>
        /// <param name="instance">Existing instance of T to save values to source.</param>
        /// <exception cref="ConfigManagerException">Thrown when a value couldn't be set.</exception>
        /// <exception cref="NotSupportedException">Thrown when the source does not support saving config values.</exception>
        public static void SaveConfig(IConfigSource source, T instance)
        {
            if (!source.CanSet)
            {
                throw new NotSupportedException(string.Format("Source '{0}' does not support saving config values.", source.GetType().Name));
            }

            foreach (var key in getConfigKeys(source, typeof(T)))
            {
                try
                {
                    object value = key.GetValue(instance, null);

                    if (getIsRequired(key))
                    {
                        if (!key.PropertyType.IsValueType)
                        {
                            if (value == null)
                            {
                                throw new ConfigManagerException(string.Format("Value is required but was not specified.", key.Name));
                            }
                        }
                    }

                    source.SetValue(getKeyName(key), value, typeof(T), key.PropertyType);
                }
                catch (Exception e)
                {
                    throw new ConfigManagerException(string.Format("Failed to set source value for key '{0}' See inner exception.", key.Name), e);
                }
            }
        }

        private static string getKeyName(PropertyInfo keyProperty)
        {
            var entryAttribute = (ConfigKeyAttribute)keyProperty.GetCustomAttributes
                (typeof(ConfigKeyAttribute), true).First();

            if (entryAttribute.KeyName != null)
            {
                return entryAttribute.KeyName;
            }
            else
            {
                return keyProperty.Name;
            }
        }

        private static object getDefaultValue(PropertyInfo keyProperty)
        {
            var defaultAttribute = (DefaultValueAttribute)keyProperty.GetCustomAttributes
                (typeof(DefaultValueAttribute), true).FirstOrDefault();

            if (defaultAttribute == null)
            {
                var runtimeDefault = new RuntimeDefaultCheck();
                return runtimeDefault.GetDefault(keyProperty.PropertyType);
            }
            else
            {
                return defaultAttribute.Value;
            }
        }

        private static bool getIsRequired(PropertyInfo keyProperty)
        {
            var entryAttribute = (ConfigKeyAttribute)keyProperty.GetCustomAttributes
                (typeof(ConfigKeyAttribute), true).First();

            return entryAttribute.Required;
        }

        private static IEnumerable<PropertyInfo> getConfigKeys(IConfigSource source, Type type)
        {
            var keys = new List<PropertyInfo>();

            foreach (var prop in type.GetProperties(PROP_BINDINGS))
            {
                var entryAttribute = prop.GetCustomAttributes(typeof(ConfigKeyAttribute), true).FirstOrDefault();

                if (entryAttribute != null)
                {
                    if (!prop.CanRead)
                    {
                        throw new ConfigManagerException("All config properties must be readable.");
                    }

                    var indexArray = prop.GetIndexParameters();

                    if (indexArray != null && indexArray.Count() > 0)
                    {
                        throw new NotSupportedException("Indexers are not supported.");
                    }

                    if (!source.GetCanHandle(prop.PropertyType))
                    {
                        throw new NotSupportedException(
                            string.Format("The type specified for config key '{0}' is not supported for source type '{1}'", 
                            prop.Name, source.GetType().Name));
                    }

                    keys.Add(prop);
                }
            }

            return keys;
        }

    }
}
