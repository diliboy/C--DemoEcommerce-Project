namespace ApiGateway.Presentation.Middelware
{
    public class AttachSignatureToRequest(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers["API-Gateway"] = "Signed";
            await next(context);
        }
    }
}
