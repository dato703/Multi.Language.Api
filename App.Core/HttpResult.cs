using System.Collections.Generic;
using System.Linq;

namespace App.Core
{
    public class HttpResult
    {
        public string Message { get; set; }
        public HttpResultStatus Status { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        public HttpResult()
        {

        }

        public HttpResult(HttpResultStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public HttpResult(string parameterName, object data, HttpResult result)
        {
            if (result.Parameters.Any())
            {
                Parameters.Add(parameterName, data);
            }
            else
            {
                Parameters = new Dictionary<string, object> { { parameterName, data } };
            }

            Status = result.Status;
            Message = result.Message;
        }

        public static HttpResult Unauthorized()
        {
            return new HttpResult
            {
                Status = HttpResultStatus.Unauthorized,
                Message = "თქვენ არ ხართ ავტორიზებული, გთხოვთ გაიაროთ ავტორიზაცია",
                Parameters = new Dictionary<string, object>
                {
                    { "unauthorized", "true" }
                }
            };
        }

        public static HttpResult AccessDenied()
        {
            return new HttpResult
            {
                Status = HttpResultStatus.AccessDenied,
                Message = "თქვენ არ გაქვთ წვდომა მოთხოვნაზე",
                Parameters = new Dictionary<string, object>
                {
                    { "access-denied", "true" }
                }
            };
        }
    }

    public static class HttpResultExtensions
    {
        public static HttpResult AddParameter(this HttpResult httpResult, string parameterName, object data)
        {
            httpResult.Parameters.Add(parameterName, data);
            httpResult.Status = HttpResultStatus.Success;
            return httpResult;
        }
        public static HttpResult Successful(this HttpResult httpResult)
        {
            httpResult.Status = HttpResultStatus.Success;
            return httpResult;
        }

        public static HttpResult Successful(this HttpResult httpResult, string message)
        {
            httpResult.Status = HttpResultStatus.Success;
            httpResult.Message = message;
            return httpResult;
        }

        public static HttpResult Error(this HttpResult httpResult)
        {
            httpResult.Status = HttpResultStatus.Error;
            return httpResult;
        }

        public static HttpResult Error(this HttpResult httpResult, string message)
        {
            httpResult.Status = HttpResultStatus.Error;
            httpResult.Message = message;
            return httpResult;
        }

        public static HttpResult Error(this HttpResult httpResult, string message, HttpResultStatus httpResultStatus)
        {
            httpResult.Status = httpResultStatus;
            httpResult.Message = message;
            return httpResult;
        }
    }
}
