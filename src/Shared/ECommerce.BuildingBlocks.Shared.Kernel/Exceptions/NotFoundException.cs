using System.Net;

namespace ECommerce.BuildingBlocks.Shared.Kernel.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message)
        : base(message, (int)HttpStatusCode.NotFound) { }
}
