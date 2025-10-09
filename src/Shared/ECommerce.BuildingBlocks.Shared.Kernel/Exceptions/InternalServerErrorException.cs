using System.Net;

namespace ECommerce.BuildingBlocks.Shared.Kernel.Exceptions;

public sealed class InternalServerErrorException : BaseException
{
    public InternalServerErrorException(string message)
        : base(message, (int)HttpStatusCode.InternalServerError) { }
}
