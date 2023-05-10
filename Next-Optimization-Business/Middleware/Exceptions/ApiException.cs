using System.Net;

namespace NextOptimization.Business.Middleware
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public ApiException(string message) : base(message)
        {
        }
    }
}