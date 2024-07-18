using System.Text.Json.Serialization;

namespace Dima.Core.Responses
{
    public class Response<T>
    {
        private const int DefaultStatusCode = 200;
        private readonly int _code = DefaultStatusCode;
        public T? Data { get; set; }
        public string? Message { get; set; }

        [JsonConstructor]
        public Response() => _code = DefaultStatusCode;

        public Response(T? data, int code = DefaultStatusCode, string? message = null)
        {
            Data = data;
            Message = message;
            _code = code;
        }

        [JsonIgnore]
        public bool IsSuccess => _code is >= 200 and < 300;
    }
}
