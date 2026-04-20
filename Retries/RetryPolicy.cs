using System;
using Logging;
using Server;
using Server.Models;

namespace Retries
{
    public class RetryPolicy : IServerInterface
    {
        private readonly ILogger _logger;
        private readonly int _maxRetries;
        private readonly IServerInterface _inner;

        public RetryPolicy(ILogger logger, int maxRetries, IServerInterface inner)
        {
            _logger = logger;
            _maxRetries = maxRetries;
            _inner = inner;
        }

        public OperationResult DoOperation()
        {
            for (int attempt = 1; attempt <= _maxRetries; attempt++)
            {
                try
                {
                    OperationResult result = _inner.DoOperation();

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
