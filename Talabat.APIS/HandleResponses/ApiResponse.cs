namespace Talabat.APIS.HandleResponses
{
    public class ApiResponse
    {

        public ApiResponse(int statusCode, string message = null) 
        {
            StatusCode = statusCode;
            Message = message ?? GetDefultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefultMessageForStatusCode(int status) 
        {
            return status switch
            {
                400 => "Bad Request",
                401 => "You are not authorized",
                404 => "Resource not found",
                500 => "Internal server error",
                _ => null
            };
        }
    }
}
