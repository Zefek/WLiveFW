using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WLive.WLiveObjects;

namespace WLive
{
    class AGetOpConverter<T> : JsonConverter
    {
        public static JsonSerializerSettings Settings
        {
            get
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new AGetOpConverter<T>());
                settings.Formatting = Formatting.Indented;
                return settings;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            try
            {
                object value = Activator.CreateInstance(objectType);
                foreach (System.Reflection.PropertyInfo mi in value.GetType().GetProperties())
                    foreach (object o in mi.GetCustomAttributes(true))
                        if (o.GetType().Name == "WLiveAttribute")
                            return true;
            }
            catch
            {
                throw;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            IEnumerator<KeyValuePair<string, JToken>> enumerator = jo.GetEnumerator();
            object value = Activator.CreateInstance(objectType);
            bool first = true;
            while (enumerator.MoveNext())
            {
                System.Reflection.PropertyInfo property = null;
                bool found = false;
                foreach (System.Reflection.PropertyInfo propertyInfo in value.GetType().GetProperties())
                {
                    foreach (object customAttribute in propertyInfo.GetCustomAttributes(true))
                    {
                        if (customAttribute is WLiveAttribute)
                        {
                            WLiveAttribute attribute = (WLiveAttribute)customAttribute;
                            if (string.Compare(attribute.Name.ToUpper(CultureInfo.InvariantCulture), enumerator.Current.Key.ToUpper(CultureInfo.InvariantCulture), StringComparison.Ordinal) == 0)
                            {
                                found = true;
                                property = propertyInfo;
                                break;
                            }
                        }
                    }
                    if (found)
                        break;
                }

                if (enumerator.Current.Value.Type == JTokenType.Array)
                {
                    if (property == null && first)
                    {
                        List<T> lt = new List<T>();
                        foreach (JToken token in enumerator.Current.Value.Children())
                        {
                            object vl = JsonConvert.DeserializeObject(token.ToString(), value.GetType(), Settings);
                            if (vl is T)
                                lt.Add((T)vl);
                            else
                                lt.AddRange((IEnumerable<T>)vl);
                        }
                        return lt;
                    }
                    else
                    { }
                }
                else if (enumerator.Current.Value.Type == JTokenType.Object && property != null)
                {
                    object vl = JsonConvert.DeserializeObject(enumerator.Current.Value.ToString(), property.PropertyType, Settings);
                    property.SetValue(value, Convert.ChangeType(vl, property.PropertyType, CultureInfo.InvariantCulture), null);
                    first = false;
                }
                else
                {
                    if (property != null)
                    {
                        property.SetValue(value, Convert.ChangeType(enumerator.Current.Value, property.PropertyType, CultureInfo.InvariantCulture), null);
                        first = false;
                    }
                }
            }
            return value;
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (value == null)
                throw new ArgumentNullException("value");
            writer.WriteStartObject();
            JToken jo = JToken.FromObject(value);
            foreach (System.Reflection.PropertyInfo mi in value.GetType().GetProperties())
            {
                foreach (object o in mi.GetCustomAttributes(true))
                {
                    if (o is WLiveAttribute)
                    {
                        WLiveAttribute wa = (WLiveAttribute)o;
                        if (wa.Writable)
                        {
                            if (jo[mi.Name].Type == JTokenType.Array)
                            {
                                writer.WritePropertyName(wa.Name);
                                writer.WriteStartArray();
                                List<string> arrayData = new List<string>();
                                foreach (var loop in (dynamic)mi.GetValue(value, null))
                                {
                                    string data = JsonConvert.SerializeObject(loop, Settings);
                                    arrayData.Add(data);
                                }
                                writer.WriteRaw(string.Join(", ", arrayData));
                                writer.WriteEndArray();
                            }
                            else if (jo[mi.Name].Type == JTokenType.Object)
                            {
                                writer.WritePropertyName(wa.Name);
                                string data = JsonConvert.SerializeObject(mi.GetValue(value, null), Settings);
                                writer.WriteRawValue(data);
                            }
                            else
                            {
                                if (jo[mi.Name].Type != JTokenType.Null)
                                {
                                    object propertyValue = mi.GetValue(value, null);
                                    if (propertyValue is string)
                                    {
                                        if (!string.IsNullOrEmpty((string)propertyValue))
                                        {
                                            writer.WritePropertyName(wa.Name);
                                            writer.WriteValue(mi.GetValue(value, null));
                                        }
                                    }
                                    else
                                    {
                                        writer.WritePropertyName(wa.Name);
                                        writer.WriteValue(mi.GetValue(value, null));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            writer.WriteEndObject();
        }
    }
}
