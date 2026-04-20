using Logging;
using Server;
using Server.Models;

namespace Retries
{
    internal static class Program
    {
        private static void Main()
        {
            ILogger logger = new ConsoleLogger();
            IServerInterface serverInterface = new RetryPolicy(logger, 3, new ServerInterface(logger));

            OperationResult result = serverInterface.DoOperation();

            logger.LogMessage(result.HasError ? $"Operation failed: {result.ErrorMsg}" : "Operation succeeded.");
        }
    }
}
