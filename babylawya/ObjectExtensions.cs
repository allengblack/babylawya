using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace babylawya
{
    public static class ExtensionMethods
    {
        public static bool IsNull<T>(this T source)
        {
            return source == null;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return (source == null || !source.Any());
        }

        public static bool IsAnyOf<T>(this T source, params T[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return false;
            }

            return values.Contains(source);
        }

        //public static string SettingValue(this string source)
        //{
        //    return ConfigurationManager.AppSettings[source];
        //}

        //public static string ConnectionString(this string source)
        //{
        //    var connectionString = ConfigurationManager.ConnectionStrings[source];

        //    if (connectionString != null)
        //    {
        //        return connectionString.ConnectionString;
        //    }

        //    return null;
        //}

        public static bool IsOne<T>(this IEnumerable<T> source)
        {
            return (source.Count() == 1);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static T Second<T>(this IEnumerable<T> source)
        {
            return source.ElementAt(1);
        }

        public static T Third<T>(this IEnumerable<T> source)
        {
            return source.ElementAt(2);
        }

        public static bool IsTrue(this string source)
        {
            if (source.IsNullOrEmpty())
            {
                return false;
            }

            return source.ToLower().IsAnyOf("1", "true", "on");
        }

        public static int ToSafeInt(this string source, int defaultValue = 0)
        {
            int value = defaultValue;
            int.TryParse(source, out value);

            return value;
        }

        public static long ToSafeLong(this string source, long defaultValue = 0)
        {
            long value = defaultValue;
            long.TryParse(source, out value);

            return value;
        }

        public static void Upsert<T, S>(this IDictionary<S, T> source, S key, T value)
        {
            if (source.ContainsKey(key))
            {
                source[key] = value;
            }
            else
            {
                source.Add(key, value);
            }
        }

        public static S TryGet<T, S>(this IDictionary<T, S> source, T key, S value)
        {
            if (source.ContainsKey(key))
                return source[key];

            return value;
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            var result = default(T);

            if (!Enum.TryParse(value, out result))
                throw new ArgumentException("value is an invalid enum type");

            return result;

        }

        public static string ToJson<T>(this T source)
        {
            return JsonConvert.SerializeObject(source);
        }

        public static T FromJson<T>(this string source)
        {
            return JsonConvert.DeserializeObject<T>(source);
        }

        public static string ToParams<S>(this S obj)
        {
            var type = obj.GetType();

            if (obj == null)
                return string.Empty;

            if (type.Name == "String")
                return obj.ToString();

            if (obj is ICollection)
                return ToParams((ICollection)obj);

            var props = type.GetProperties();
            var collection = new NameValueCollection();

            foreach (var prop in props)
            {
                var value = prop.GetValue(obj);

                if (value == null)
                    continue;

                collection.Add(prop.Name, value.ToString());
            }

            return ToParams(collection);
        }

        public static string ToParams(this ICollection data)
        {
            if (data == null || data.Count == 0)
                return string.Empty;

            var sb = new StringBuilder("?");

            foreach (KeyValuePair<string, string> item in data)
            {
                if (item.Value == null)
                    continue;

                sb.Append(item.Key);
                sb.Append("=");
                sb.Append(Uri.EscapeUriString(item.Value));
                sb.Append("&");
            }

            return sb.ToString().TrimEnd('&');
        }

        public static void MergeShallow<T, S>(this T target, S source, params string[] ignoreProperties)
        {
            if (target.IsNull() || source.IsNull())
            {
                return;
            }

            var type = typeof(S);
            var props = type.GetProperties();

            foreach (var prop in props)
            {

                if (!prop.CanWrite
                    || prop.PropertyType.IsArray
                    || prop.PropertyType.IsInterface
                    || (ignoreProperties != null && ignoreProperties.Contains(prop.Name))
                    || (prop.PropertyType.IsClass && prop.PropertyType.Name != "String")) // TODO: this is meant to exlude only the custom classes 
                {
                    continue;
                }

                var value = prop.GetValue(source);
                prop.SetValue(target, value);
            }
        }

        public static string BreakupText(this string source)
        {
            if (source.IsNullOrEmpty() || source.IsOne())
            {
                return source;
            }

            var charList = new List<char> { source[0] };

            for (int i = 1; i < source.Length; i++)
            {
                if (char.IsUpper(source[i]))
                {
                    charList.Add(' ');
                }

                charList.Add(source[i]);
            }

            return new string(charList.ToArray());
        }
    }
}
