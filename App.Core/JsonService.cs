using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace App.Core
{
    public static class JsonService
    {
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {

                return default(T);
            }

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                DateFormatString = SystemSettings.ShortDatePattern
            };
            settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = SystemSettings.LongDatePattern });
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static T DeserializeSearchJson<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                DateFormatString = SystemSettings.ShortDatePattern,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
            settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = SystemSettings.LongDatePattern });
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static object Deserialize(string json, Type type, bool typeNameHandling = true)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            var settings = new JsonSerializerSettings
            {
                DateFormatString = SystemSettings.ShortDatePattern
            };

            if (typeNameHandling)
            {
                settings.TypeNameHandling = TypeNameHandling.Objects;
            }

            settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = SystemSettings.LongDatePattern });
            return JsonConvert.DeserializeObject(json, type, settings);
        }

        public static string Serialize(object value, bool typeNameHandling = true)
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = SystemSettings.ShortDatePattern
            };

            if (typeNameHandling)
            {
                settings.TypeNameHandling = TypeNameHandling.Objects;
            }

            settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = SystemSettings.LongDatePattern });
            var json = JsonConvert.SerializeObject(value, settings);

            return json;
        }

        public static T GetPropertyValue<T>(string json, string propertyPath)
        {
            var jo = JObject.Parse(json);
            var token = jo.SelectToken(propertyPath);
            return token.ToObject<T>();
        }

        public static T DeepCopy<T>(T obj, bool typeNameHandling = true)
        {
            var json = Serialize(obj, typeNameHandling);
            return Deserialize<T>(json);
        }

        public static string MergeObjectJson<T>(T primary, T source)
        {
            var sourceObj = JObject.Parse(Serialize(source));
            var primaryVersion1 = JObject.Parse(Serialize(primary));
            var primaryFinalVersion = JObject.Parse(Serialize(primary));

            primaryFinalVersion.Merge(sourceObj, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union,
                MergeNullValueHandling = MergeNullValueHandling.Ignore
            });

            primaryFinalVersion.Merge(primaryVersion1, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union,
                MergeNullValueHandling = MergeNullValueHandling.Ignore
            });

            return primaryFinalVersion.ToString();
        }
    }

    public static class SystemSettings
    {
        public const string ShortDatePattern = "dd-MM-yyyy";
        public const string LongDatePattern = "dd-MM-yyyy HH:mm:ss.FFFFFFFK";
        public const string LongDateElasticPattern = "yyyy-MM-ddTHH:mm:ss.FFFFFFFK";
        public const string DateShortTimePattern = "dd-MM-yyyy HH:mm";
        public const string DateTimeShortTimePattern = "dd-MM-yyyy HH:mm:ss";
        public const string PdfReportDateTimeShortTimePattern = "dd.MM.yyyy HH:mm:ss";
        public const string PdfReportDateTimeShortDatePattern = "dd.MM.yyyy";
        public const string PdfReportDateShortTimePattern = "dd.MM.yyyy HH:mm";
        public const string PdfReportDatePattern = "dd.MM.yyyy";
        public const string ShortTimePattern = "HH:mm";
        public const string LongTimePattern = "HH:mm:ss";
        public const string NumberDecimalSeparator = ".";
        public const string NumberGroupSeparator = " ";
        public const string DatePatternForMomentjs = "yyyy-MM-dd HH:mm:ss";
        public const string MomentToDateTime = "ddd MMM dd yyyy HH:mm:ss";
        public static string FullDateTimePattern = "dd-MM-yyyyTHH:mm:ss.FFFFFFFK";
    }
}
