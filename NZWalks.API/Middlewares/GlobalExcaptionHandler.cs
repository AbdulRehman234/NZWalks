using System.Net;

namespace NZWalks.API.Middlewares
{
    public class GlobalExcaptionHandler
    {
        public ILogger<GlobalExcaptionHandler> _logger { get; }
        public RequestDelegate _next { get; }

        public GlobalExcaptionHandler(ILogger<GlobalExcaptionHandler> logger,RequestDelegate requestDelegate)
        {
            _logger = logger;
            _next = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {

                var errorId =  Guid.NewGuid(); //this is line used to check that you can through id which section is id where is this id in file 
                _logger.LogError(ex, $"{errorId} : {ex.Message}"); //this line show a message inside file with id and message 

                //Return a Custom Error Response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong !"
                };
                await httpContext.Response.WriteAsJsonAsync(error);//this funcition automatically convert in to json  and this httpcontxt return not used here 
                //now inject thses middle ware inside program .cs
            }
        }

    }
}
