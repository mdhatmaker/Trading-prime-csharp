using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CryptoTools.CryptoSystem
{
    public class Reflection
    {

        // Return all get/set public properties for a given type T
		public static List<PropertyInfo> GetProperties<T>()
		{
			List<PropertyInfo> result = new List<PropertyInfo>();

			PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in properties)
            {
                // Only work with strings
                //if (p.PropertyType != typeof(string)) { continue; }

                // If not writable then cannot null it; if not readable then cannot check it's value
                if (!p.CanWrite || !p.CanRead) { continue; }

                MethodInfo mget = p.GetGetMethod(false);
                MethodInfo mset = p.GetSetMethod(false);

                // Get and set methods have to be public
                if (mget == null) { continue; }
                if (mset == null) { continue; }

				result.Add(p);

                /*foreach (T item in list)
                {
                    if (string.IsNullOrEmpty((string)p.GetValue(item, null)))
                    {
                        p.SetValue(item, replacement, null);
                    }
                }*/
            }
			return result;
		}
        
		public static object GetProperty<T>(object src, string propertyName) where T : class
		{
            return src.GetType().GetProperty(propertyName).GetValue(src, null);
		}

		public static void SetProperty<T>(T obj, string propertyName, object value) where T : class
		{
			obj.GetType().InvokeMember(propertyName,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
				Type.DefaultBinder, obj, new object[] { value });
		}
        
		public static List<object> GetPropertyValues<T>(T obj) where T : class
		{
			var result = new List<object>();
			var props = Reflection.GetProperties<T>();
			foreach (var p in props)
			{
				result.Add(GetProperty<T>(obj, p.Name));
			}
			return result;
		}
        
		public static void SetPropertyValues<T>(T obj, string csvLine) where T : class
        {
            var props = Reflection.GetProperties<T>();
			var split = csvLine.Split(',');
			for (int i = 0; i < props.Count; ++i)
			{
				Type ptype = props[i].PropertyType;
				var value = Convert.ChangeType(split[i], ptype);
				SetProperty<T>(obj, props[i].Name, value);
			}
        }


    } // end of class Reflection
} // end of namespace
