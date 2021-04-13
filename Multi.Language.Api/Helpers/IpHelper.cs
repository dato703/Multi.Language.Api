using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Multi.Language.Api.Helpers
{
    public static class IpHelper
    {
        public static bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        public static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }
        public static string GetRequestIP(this IHttpContextAccessor _httpContextAccessor, bool tryUseXForwardHeader = true)
        {
            string ip = null;

            //
            if (tryUseXForwardHeader)
                ip = _httpContextAccessor.GetHeaderValueAs<string>("X-Forwarded-For").SplitCsv().FirstOrDefault();

            if (!ValidateIPv4(ip))
            {
                ip = null;
            }
            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
            {
                ip = $"{_httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()}";
            }

            return ip.Trim();
        }

        public static T GetHeaderValueAs<T>(this IHttpContextAccessor _httpContextAccessor, string headerName)
        {
            StringValues values = default;

            if (_httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }
    }
}
