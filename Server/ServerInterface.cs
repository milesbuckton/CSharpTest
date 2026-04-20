using System;
using Logging;
using Server.Models;

namespace Server
{
    public class ServerInterface : IServerInterface
    {
        private readonly ILogger _logger;

        public ServerInterface(ILogger logger)
        {
            _logger = logger;
        }

        public OperationResult DoOperation()
        {
            try
            {
                _logger.LogMessage("Going to do the operation now.");

                // Do the operation that sometimes times out and throws an exception.

                _logger.LogMessage("Did the operation successfully?");

                return OperationResult.Success;
            }
            catch (Exception exc)
            {
                _logger.LogMessage($"Error while doing the operation: {exc.Message}.");

                return OperationResult.Error(exc.Message);
            }
        }
    }
}
