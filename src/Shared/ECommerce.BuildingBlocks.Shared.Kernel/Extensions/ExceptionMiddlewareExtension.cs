using ECommerce.BuildingBlocks.Shared.Kernel.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ECommerce.BuildingBlocks.Shared.Kernel.Extensions;

public static class ExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionMiddleware(IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}
