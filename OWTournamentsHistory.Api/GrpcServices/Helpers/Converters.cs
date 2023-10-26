using Grpc.Core;
using OWTournamentsHistory.Api.Services.Contract.Exceptions;

namespace OWTournamentsHistory.Api.GrpcServices.Helpers
{
    internal static class Converters
    {
        public static RpcException ToRpcException(this ServiceException exception) => exception switch
        {
            NotFoundException => new (new Status(StatusCode.NotFound, exception.Message, exception)),
            InvalidParametersException => new (new Status(StatusCode.InvalidArgument, exception.Message, exception)),

            _ => exception.ToGenericRpcException(),
        };

        public static RpcException ToGenericRpcException(this Exception exception) =>
            new(new Status(StatusCode.Unknown, exception.Message, exception));

        public static async Task<T> WrapRpcCall<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch(ServiceException ex)
            {
                throw ex.ToRpcException();
            }
            catch (Exception ex)
            {
                throw ex.ToGenericRpcException();
            }
        }
    }
}
