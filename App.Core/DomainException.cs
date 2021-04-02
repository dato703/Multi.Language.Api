using System;

namespace App.Core
{
    public class DomainException : Exception
    {
        public dynamic DeveloperInfo { get; protected set; }
        public ExceptionLevel Level { get; protected set; }

        public DomainException(string message, ExceptionLevel level, dynamic developerInfo = null, Exception innerException = null) : base(message, innerException)
        {
            Level = level;
            DeveloperInfo = developerInfo;
        }
    }

    public enum ExceptionLevel
    {
        Info = 1,
        Warning = 2,
        Error = 3,
        Fatal = 4,
        Security = 5
    }
}
