using System.Net;
using Newtonsoft.Json;
using Models;
using CustomExceptions;

namespace Middleware
{
    public class ErrorHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            ApiResponse<object> apiResponse;

            switch (exception)
            {
                case NotFoundException notFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    apiResponse = new ApiResponse<object>
                    {
                        StatusCode = 404,
                        Status = "fail",
                        Message = notFoundException.Message,
                        Data = null
                    };
                    break;
                case ValidationException validationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    apiResponse = new ApiResponse<object>
                    {
                        StatusCode = 400,
                        Status = "fail",
                        Message = validationException.Message,
                        Data = null
                    };
                    break;
                case UnauthorizedException unauthorizedException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    apiResponse = new ApiResponse<object>
                    {
                        StatusCode = 401,
                        Status = "fail",
                        Message = unauthorizedException.Message,
                        Data = null
                    };
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    apiResponse = new ApiResponse<object>
                    {
                        StatusCode = 500,
                        Status = "error",
                        Message = "An unexpected error occurred.",
                        Data = null
                    };
                    break;
            }

            var result = JsonConvert.SerializeObject(apiResponse);
            return response.WriteAsync(result);
        }
    }
}