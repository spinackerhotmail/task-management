namespace TaskManagementService.CommonLib.Exceptions
{
    public class ExternalApiException : Exception
    {
        public string ServiceName { get; private set; }
        public int StatusCode { get; private set; }
        public string Response { get; private set; }
        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

        public ExternalApiException(string serviceName, string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException)
        : base(message, innerException)
        {
            ServiceName = serviceName;
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }
    }
}
