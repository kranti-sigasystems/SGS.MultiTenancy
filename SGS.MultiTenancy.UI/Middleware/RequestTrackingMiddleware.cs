using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SGS.MultiTenancy.UI.Middleware
{
    public class RequestTrackingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestTrackingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get &&
                !context.Request.Path.StartsWithSegments("/ErrorP"))
            {
                context.RequestServices
                    .GetRequiredService<ITempDataDictionaryFactory>()
                    .GetTempData(context)["ReturnUrl"] =
                        context.Request.Path + context.Request.QueryString;
            }

            await _next(context);
        }
    }

}
