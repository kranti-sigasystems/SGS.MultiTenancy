namespace SGS.MultiTenancy.UI.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var host = context.Request.Host.Host;
            var subdomain = host.Split('.').FirstOrDefault();

            if (string.IsNullOrEmpty(subdomain) || subdomain == "localhost")
            {
                await _next(context);
                return;
            }

            context.Items["TenantSlug"] = subdomain;
            await _next(context);
        }
    }
}
