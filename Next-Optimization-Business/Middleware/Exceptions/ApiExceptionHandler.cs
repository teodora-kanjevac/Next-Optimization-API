using System.Net;

namespace NextOptimization.Business.Middleware
{
    public class ApiExceptionHandler
    {
        public static void ThrowApiException(HttpStatusCode statusCode, string message)
        {
            ApiException apiException = new(message)
            {
                StatusCode = statusCode
            };

            throw apiException;
        }

        public static void StringNotNullOrEmpty(string field, string fieldName)
        {
            if (string.IsNullOrEmpty(field))
            {
                ThrowApiException(HttpStatusCode.BadRequest, fieldName + " is null or empty.");
            }
        }

        public static void ObjectNotNull(object obj, string message)
        {
            if (obj == null)
            {
                ThrowApiException(HttpStatusCode.BadRequest, message + " doesn't exist.");
            }
        }
    }
}