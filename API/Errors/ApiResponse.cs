using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return  statusCode switch
            {
                400 => "You have made a bad request to the server",
                401 => "You are unauthorized to enter this site",
                404 => "Unable to find resource",
                500 => "Server Error",
                _ => null
            };
        }
    }
}