using System;

namespace Console
{
    internal sealed class ConsoleLogger : Contract.ILogger
    {
        public void Info(string message)
        {
            System.Console.WriteLine(message);
        }

        public void Error(string message, Exception ex)
        {
            System.Console.Error.WriteLine(message);
        }
    }
}