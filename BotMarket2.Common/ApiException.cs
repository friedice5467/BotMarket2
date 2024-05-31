using System.Net;
using System.Text.Json;

namespace BotMarket2.Common
{
    public class ApiException : Exception
    {

        private HttpStatusCode _statusCode { get; set; }

        public HttpStatusCode StatusCode { get => _statusCode; }

        private string? _params { get; set; }
        public string? Params { get => _params; }

        public ApiException(HttpStatusCode statusCode) : base(string.Empty)
        {
            _statusCode = statusCode;
        }

        public ApiException(HttpStatusCode statusCode, string Message) : base(Message)
        {
            _statusCode = statusCode;
        }

        public ApiException(HttpStatusCode statusCode, string Message, string? Params) : base(Message)
        {
            _statusCode = statusCode;
            _params = Params;
        }

        public ApiException(HttpResponseMessage response)
        {
            var payload = response.Content.ReadAsStringAsync().Result;
            var error = JsonSerializer.Deserialize<ErrorModelDTO>(payload);
            var message = error?.Message;
            var params_ = error?.Params;
            if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
                throw new ApiException(response.StatusCode, message, params_);
            else
                throw new ApiException(response.StatusCode);
        }
    }

    public class ErrorModelDTO
    {
        public int? ErrorCode { get; set; }
        public string Message { get; set; } = null!;
        public string? Params { get; set; }
    }
}

