using System;

namespace Logging
{
    public class ConsoleLogger : ILogger
    {
        public void LogMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
