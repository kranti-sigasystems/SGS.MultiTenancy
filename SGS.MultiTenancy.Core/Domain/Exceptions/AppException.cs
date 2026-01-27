using System.Net;

namespace SGS.MultiTenancy.Core.Domain.Exceptions
{
    public class AppException : Exception
    {
        public int StatusCode { get; }

        public AppException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            StatusCode = (int)statusCode;
        }
    }
}