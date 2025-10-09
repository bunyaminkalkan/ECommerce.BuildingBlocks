using System.Net;

namespace ECommerce.BuildingBlocks.Shared.Kernel.Exceptions;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException(string message)
        : base(message, (int)HttpStatusCode.Unauthorized) { }
}
