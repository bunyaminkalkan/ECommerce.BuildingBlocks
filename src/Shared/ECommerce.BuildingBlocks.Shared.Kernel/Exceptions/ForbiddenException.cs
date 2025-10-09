using System.Net;

namespace ECommerce.BuildingBlocks.Shared.Kernel.Exceptions;

public class ForbiddenException : BaseException
{
    public ForbiddenException(string message)
        : base(message, (int)HttpStatusCode.Forbidden) { }
}
