using System;
using System.Linq.Expressions;

namespace DBA.FreshdeskSharp.Exceptions
{
    internal class ExpressionTypeNotImplementedException : Exception
    {
        public ExpressionTypeNotImplementedException(Expression exp) : base(MessageFromExp(exp)) { }

        private static string MessageFromExp(Expression exp)
        {
            return "FreshdeskSharp can only handle simple search queries.  Please " +
                   "simplify your query or use the string version of the search " +
                   $"function. \r\n\r\nExpression: {exp}";
        }
    }
}