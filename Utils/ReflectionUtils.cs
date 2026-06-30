using System.Reflection;

namespace Library.Utils
{
    /// <summary>
    /// Provides static helpers for reading and writing object members via reflection.
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        /// Gets the value of a property using reflection.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="obj">The object containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of the property, or the default value of T if the property is not found or an error occurs.</returns>
        public static T GetPropertyValue<T>(object? obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
            {
                return default;
            }

            try
            {
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    var value = propertyInfo.GetValue(obj);
                    if (value != null && value is T)
                    {
                        return (T)value;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                Console.WriteLine($"Error getting property '{propertyName}': {ex.Message}");
            }

            return default;
        }

        /// <summary>
        /// Sets the value of a property using reflection.
        /// </summary>
        /// <param name="obj">The object containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            try
            {
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(obj, Convert.ChangeType(value, propertyInfo.PropertyType));
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                Console.WriteLine($"Error setting property '{propertyName}': {ex.Message}");
            }
        }

        /// <summary>
        /// Invokes a method using reflection.
        /// </summary>
        /// <param name="obj">The object containing the method.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="parameters">The parameters to pass to the method.</param>
        /// <returns>The return value of the method, or null if the method is void or an error occurs.</returns>
        public static object InvokeMethod(object obj, string methodName, params object[] parameters)
        {
            if (obj == null || string.IsNullOrEmpty(methodName))
            {
                return null;
            }

            try
            {
                var methodInfo = obj.GetType().GetMethod(methodName);
                if (methodInfo != null)
                {
                    return methodInfo.Invoke(obj, parameters);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                Console.WriteLine($"Error invoking method '{methodName}': {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Creates an instance of a type using reflection.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <returns>An instance of the type, or default(T) if an error occurs.</returns>
        public static T CreateInstance<T>() where T : class, new()
        {
            try
            {
                return Activator.CreateInstance<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating instance of {typeof(T).Name}: {ex.Message}");
                return default;
            }
        }

        /// <summary>
        /// Gets all properties of an object.
        /// </summary>
        /// <param name="obj">The object to get properties from.</param>
        /// <returns>An array of PropertyInfo objects.</returns>
        public static PropertyInfo[] GetProperties(object obj)
        {
            if (obj == null) return null;
            return obj.GetType().GetProperties();
        }
    }
}