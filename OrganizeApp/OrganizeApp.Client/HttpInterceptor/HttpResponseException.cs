using System.Text.Json.Serialization;

namespace OrganizeApp.Client.HttpInterceptor
{
    [Serializable]
    public class HttpResponseException : Exception
    {
        public HttpResponseException()
        {

        }

        public HttpResponseException(string message) : base(message)
        {

        }
    }
}
