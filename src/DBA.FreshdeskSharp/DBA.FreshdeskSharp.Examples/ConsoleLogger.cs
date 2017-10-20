using System;
using DBA.FreshdeskSharp.Models.Abstractions;

namespace DBA.FreshdeskSharp.Examples
{
    public class ConsoleLogger : IFreshdeskClientLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}