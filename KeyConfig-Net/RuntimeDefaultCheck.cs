using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyConfig
{
    /// <summary>
    /// Allows you to call default(T) with a runtime type.
    /// </summary>
    public class RuntimeDefaultCheck
    {
        /// <summary>
        /// Returns the default value of a type.
        /// </summary>
        /// <param name="t">Type to use.</param>
        /// <returns>The default value.</returns>
        public object GetDefault(Type t)
        {
            return this.GetType().GetMethod("GetDefaultGeneric").MakeGenericMethod(t).Invoke(this, null);
        }

        /// <summary>
        /// Gets the default value of T
        /// </summary>
        /// <typeparam name="T">Type generic to use.</typeparam>
        /// <returns>The default value.</returns>
        public T GetDefaultGeneric<T>()
        {
            return default(T);

            
        }
    }
}
