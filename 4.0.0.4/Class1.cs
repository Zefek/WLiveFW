using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive
{

    public class Filter
    {
        int ftype;
        public Filter(FilterTypes filterType)
        {
            ftype = (int)filterType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns string representation of filter</returns>
        public string GetTypesForQuery
        {
            get
            {
                string s = "";
                int k = ftype;
                int c = 32;
                while (k > 0)
                {
                    if (k - c >= 0)
                    {
                        switch (c)
                        {
                            case 1: s += "albums,"; break;
                            case 2: s += "audio,"; break;
                            case 4: s += "folders,"; break;
                            case 8: s += "photos,"; break;
                            case 16: s += "videos,"; break;
                            case 32: s += "notebooks,"; break;
                        }
                        k = k - c;
                        c = c >> 1;
                    }
                    else
                        c = c >> 1;
                }
                if (k != 0)
                    return "";
                if (string.IsNullOrEmpty(s))
                    return "";
                return s.Substring(0, s.Length - 1);
            }
        }
    }

    public static class UrlBuilder1
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Path to object</param>
        /// <returns>Returns Url for request</returns>
        public static Uri Build(string path)
        {
            return new Uri("https://apis.live.net/v5.0/" + path + "?access_token=" + Connection.Token);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Path to object</param>
        /// <param name="parameters">Additional query parameters represents query string</param>
        /// <returns>Returns url for request</returns>
        public static Uri Build(string path, QueryParameters parameters)
        {
            if (parameters != null)
                return new Uri(Build(path).ToString() + "&" + parameters.QueryString);
            return Build(path);
        }

        public static Uri Concat(Uri url, QueryParameters parameters)
        {
            if (parameters != null)
                return new Uri(url + "?" + parameters.QueryString);
            return url;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Url to concat</param>
        /// <param name="parameters">Additional parameters represents query string</param>
        /// <returns>Returns url for request</returns>
        public static Uri Concat(string url, QueryParameters parameters)
        {
            return Concat(new Uri(url), parameters);
        }
    }

    /*
    public static class JSONDataBuilder
    {
        public static string Build(object value)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (System.Reflection.PropertyInfo mi in value.GetType().GetProperties())
            {
                foreach (object o in mi.GetCustomAttributes(true))
                {
                    if (o.GetType().Name == "WLiveAttribute")
                    {
                        WLiveAttribute wa = (WLiveAttribute)o;
                        if (wa.Writable)
                        {
                            object val = mi.GetValue(value, null);
                            if (val != null)
                            {
                                if (!string.IsNullOrEmpty(mi.GetValue(value, null).ToString()))
                                    sb.Append("\"" + wa.Name + "\": \"" + mi.GetValue(value, null) + "\"\r\n");
                            }
                        }
                    }
                }
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
    */

    public class QueryParameters
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        /// <summary>
        /// Gets query string
        /// </summary>
        public string QueryString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, string> kvp in param)
                    sb.Append(kvp.Key + "=" + kvp.Value + "&");
                return sb.ToString().Substring(0, sb.Length - 1);
            }
        }
        /// <summary>
        /// If key already exists it change value of key. Use this method to created and changed values
        /// </summary>
        /// <param name="key">Key of parameters</param>
        /// <param name="value">Value of parameters</param>
        public void Add(string key, string value)
        {
            if (!param.ContainsKey(key))
                param.Add(key, value);
            else
                param[key] = value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Index into query params</param>
        /// <returns>Returns key value pair of query params</returns>
        public KeyValuePair<string, string> this[int index]
        {
            get
            {

                if (index < param.Count && index >= 0)
                    return param.ElementAt(index);
                return new KeyValuePair<string, string>();
            }
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class WLiveAttribute : Attribute
    {
        public bool Writable { get; }
        public string Name { get; }

        public WLiveAttribute(string name)
        {
            Name = name;
            Writable = false;
        }

        public WLiveAttribute(string name, bool writable)
        {
            Name = name;
            Writable = writable;
        }
    }

}
