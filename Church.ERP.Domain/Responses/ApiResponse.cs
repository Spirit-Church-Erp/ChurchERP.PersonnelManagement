using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.ERP.Domain.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public IReadOnlyList<string> Errors { get; }


        public ApiResponse()
        {
        }

        public ApiResponse(T data, bool success = true, string message = null, int statusCode = 200)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
            Data = data;
        }

        public ApiResponse(string message, int statusCode, IReadOnlyList<string> errors)
        {
            Success = false;
            Message = message;
            StatusCode = statusCode;
            Data = default;
            Errors = errors;
        }
    }
}
