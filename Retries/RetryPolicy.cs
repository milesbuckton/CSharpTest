using System;
using Logging;
using Server;
using Server.Models;

namespace Retries
{
    public class RetryPolicy : IServerInterface
    {
        private readonly IServerInterface _server;
        private readonly int _maxRetries;
        private readonly ILogger _logger;

        public RetryPolicy(IServerInterface server, int maxRetries, ILogger logger)
        {
            _server = server;
            _maxRetries = maxRetries;
            _logger = logger;
        }

        public OperationResult DoOperation()
        {
            for (int attempt = 1; attempt <= _maxRetries; attempt++)
            {
                try
                {
                    OperationResult result = _server.DoOperation();

                    if (result.IsSuccessful)
                        return result;

                    _logger.LogMessage($"Attempt {attempt} of {_maxRetries} failed: {result.ErrorMsg}");
                }
                catch (Exception exc)
                {
                    _logger.LogMessage($"Attempt {attempt} of {_maxRetries} failed: {exc.Message}");
                }

                if (attempt == _maxRetries)
                {
                    _logger.LogMessage("All retry attempts exhausted.");
                }
            }

            return OperationResult.Error("All retry attempts exhausted.");
        }
    }
}
