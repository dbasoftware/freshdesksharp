using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using DBA.FreshdeskSharp.Exceptions;
using DBA.FreshdeskSharp.Models.Abstractions;
using DBA.FreshdeskSharp.Models.Internal;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Core
{
    internal static class FreshdeskTicketSearchQueryBuilder
    {
        private static readonly List<string> CustomFieldFuncs = new List<string>()
        {
            "CustomField",
            "CustomString",
            "CustomBool",
            "CustomDateTime",
            "CustomInt"
        };

        public static string Build(Expression<Func<IFreshdeskTicketQuery, bool>> searchQuery)
        {
            return Build<IFreshdeskTicketQuery>(searchQuery);
        }

        public static string Build<TTicketQueryFields>(Expression<Func<TTicketQueryFields, bool>> searchQuery) where TTicketQueryFields : IFreshdeskTicketQuery
        {
            var ast = ExpToAst<TTicketQueryFields>(searchQuery.Body);
            var cleanAst = Clean(null, ast);
            return AstToQueryString(cleanAst);
        }

        private static FreshdeskTicketSearchAstNode ExpToAst<TTicketQueryFields>(Expression exp) where TTicketQueryFields : IFreshdeskTicketQuery
        {
            switch (exp.NodeType)
            {
                case ExpressionType.Add:
                    return Evaluate(exp);
                case ExpressionType.AndAlso:
                    return new FreshdeskTicketSearchAstBranchNode
                    {
                        Type = AstNodeType.And,
                        Left = ExpToAst<TTicketQueryFields>(((BinaryExpression)exp).Left),
                        Right = ExpToAst<TTicketQueryFields>(((BinaryExpression)exp).Right)
                    };
                case ExpressionType.Call:
                    return new FreshdeskTicketSearchAstLeafNode
                    {
                        Type = AstNodeType.Constant,
                        Value = Expression.Lambda(exp).Compile().DynamicInvoke().ToString()
                    };
                case ExpressionType.Constant:
                    return new FreshdeskTicketSearchAstLeafNode
                    {
                        Type = AstNodeType.Constant,
                        Value = ((ConstantExpression)exp).Value
                    };
                case ExpressionType.Convert:
                    return ExpToAst<TTicketQueryFields>(((UnaryExpression)exp).Operand);
                case ExpressionType.Equal:
                    return new FreshdeskTicketSearchAstBranchNode
                    {
                        Type = AstNodeType.Eq,
                        Left = ExpToAst<TTicketQueryFields>(((BinaryExpression)exp).Left),
                        Right = ExpToAst<TTicketQueryFields>(((BinaryExpression)exp).Right)
                    };
                case ExpressionType.LessThanOrEqual:
                    return new FreshdeskTicketSearchAstBranchNode
                    {
                        Type = AstNodeType.LtEq,
                        Left = ExpToAst<TTicketQueryFields>(((BinaryExpression)exp).Left),
                        Right = ExpToAst<TTicketQueryFields>(((BinaryExpression)exp).Right)
                    };
                case ExpressionType.GreaterThanOrEqual:
                    return new FreshdeskTicketSearchAstBranchNode
                    {
                        Type = AstNodeType.GtEq,
                        Left = ExpToAst<TTicketQueryFields>(((BinaryExpression)exp).Left),
                        Right = ExpToAst<TTicketQueryFields>(((BinaryExpression)exp).Right)
                    };
                case ExpressionType.MemberAccess:
                    var memberExp = (MemberExpression)exp;
                    if (memberExp.Member.DeclaringType == typeof(TTicketQueryFields) || memberExp.Member.DeclaringType == typeof(IFreshdeskTicketQuery))
                    {
                        var jsonProp = (JsonPropertyAttribute)memberExp.Member.GetCustomAttribute(typeof(JsonPropertyAttribute), true);
                        var propName = jsonProp != null
                            ? jsonProp.PropertyName
                            : ((MemberExpression)exp).Member.Name.ToLower();
                        return new FreshdeskTicketSearchAstLeafNode
                        {
                            Type = AstNodeType.Member,
                            Value = propName
                        };
                    }
                    var value = Expression.Lambda(exp).Compile().DynamicInvoke();
                    return new FreshdeskTicketSearchAstLeafNode
                    {
                        Type = AstNodeType.Constant,
                        Value = value
                    };
                case ExpressionType.Not:
                    var unaryExp = exp as UnaryExpression;
                    return new FreshdeskTicketSearchAstUnaryNode
                    {
                        Type = AstNodeType.Not,
                        Child = ExpToAst<TTicketQueryFields>(unaryExp?.Operand)
                    };
                case ExpressionType.OrElse:
                    return new FreshdeskTicketSearchAstBranchNode
                    {
                        Type = AstNodeType.Or,
                        Left = ExpToAst<TTicketQueryFields>(((BinaryExpression)exp).Left),
                        Right = ExpToAst<TTicketQueryFields>(((BinaryExpression)exp).Right)
                    };
                case ExpressionType.Subtract:
                    return Evaluate(exp);
                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.AndAssign:
                case ExpressionType.ArrayIndex:
                case ExpressionType.ArrayLength:
                case ExpressionType.Assign:
                case ExpressionType.Block:
                case ExpressionType.Coalesce:
                case ExpressionType.Conditional:
                case ExpressionType.ConvertChecked:
                case ExpressionType.DebugInfo:
                case ExpressionType.Decrement:
                case ExpressionType.Default:
                case ExpressionType.Divide:
                case ExpressionType.DivideAssign:
                case ExpressionType.Dynamic:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.Extension:
                case ExpressionType.Goto:
                case ExpressionType.GreaterThan:
                case ExpressionType.Increment:
                case ExpressionType.Index:
                case ExpressionType.Invoke:
                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.Label:
                case ExpressionType.Lambda:
                case ExpressionType.LeftShift:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.LessThan:
                case ExpressionType.ListInit:
                case ExpressionType.Loop:
                case ExpressionType.MemberInit:
                case ExpressionType.Modulo:
                case ExpressionType.ModuloAssign:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.New:
                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                case ExpressionType.NotEqual:
                case ExpressionType.OnesComplement:
                case ExpressionType.Or:
                case ExpressionType.OrAssign:
                case ExpressionType.Parameter:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.Power:
                case ExpressionType.PowerAssign:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.Quote:
                case ExpressionType.RightShift:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.RuntimeVariables:
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Switch:
                case ExpressionType.Throw:
                case ExpressionType.Try:
                case ExpressionType.TypeAs:
                case ExpressionType.TypeEqual:
                case ExpressionType.TypeIs:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Unbox:
                    throw new ExpressionTypeNotImplementedException(exp);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static FreshdeskTicketSearchAstNode Clean(FreshdeskTicketSearchAstNode parent, FreshdeskTicketSearchAstNode node)
        {
            var branch = node as FreshdeskTicketSearchAstBranchNode;
            switch (node.Type)
            {
                case AstNodeType.And:
                    if (branch?.Left is FreshdeskTicketSearchAstLeafNode)
                    {
                        branch.Left = ConvertToBoolEq(branch.Left);
                    }
                    else if(branch != null)
                    {
                        branch.Left = Clean(branch, branch.Left);
                    }
                    if (branch?.Right is FreshdeskTicketSearchAstLeafNode)
                    {
                        branch.Right = ConvertToBoolEq(branch.Right);
                    }
                    else if(branch != null)
                    {
                         branch.Right = Clean(branch, branch.Right);
                    }
                    return branch;
                case AstNodeType.Member:
                    if (parent == null)
                    {
                        return ConvertToBoolEq(node);
                    }
                    return node;
                case AstNodeType.Or:
                    if (branch?.Left is FreshdeskTicketSearchAstLeafNode)
                    {
                        branch.Left = ConvertToBoolEq(branch.Left);
                    }
                    else if(branch != null)
                    {
                         branch.Left = Clean(branch, branch.Left);
                    }
                    if (branch?.Right is FreshdeskTicketSearchAstLeafNode)
                    {
                        branch.Right = ConvertToBoolEq(branch.Right);
                    }
                    else if(branch != null)
                    {
                         branch.Right = Clean(branch, branch.Right);
                    }
                    return branch;
                case AstNodeType.Constant:
                case AstNodeType.Eq:
                case AstNodeType.Not:
                case AstNodeType.LtEq:
                case AstNodeType.GtEq:
                    return node;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static FreshdeskTicketSearchAstNode ConvertToBoolEq(FreshdeskTicketSearchAstNode node)
        {
            return new FreshdeskTicketSearchAstBranchNode
            {
                Type = AstNodeType.Eq,
                Left = node,
                Right = new FreshdeskTicketSearchAstLeafNode
                {
                    Type = AstNodeType.Constant,
                    Value = true
                }
            };
        }

        private static string AstToQueryString(FreshdeskTicketSearchAstNode node)
        {
            var branch = node as FreshdeskTicketSearchAstBranchNode;
            var leaf = node as FreshdeskTicketSearchAstLeafNode;
            var unary = node as FreshdeskTicketSearchAstUnaryNode;
            switch (node.Type)
            {
                case AstNodeType.And:
                    var leftPrefix = branch?.Left.Type == AstNodeType.Or ? "(" : "";
                    var leftPostfix = branch?.Left.Type == AstNodeType.Or ? ")" : "";
                    var rightPrefix = branch?.Right.Type == AstNodeType.Or ? "(" : "";
                    var rightPostfix = branch?.Right.Type == AstNodeType.Or ? ")" : "";
                    var left = $"{leftPrefix}{AstToQueryString(branch?.Left)}{leftPostfix}";
                    var right = $"{rightPrefix}{AstToQueryString(branch?.Right)}{rightPostfix}";
                    return $"{left} AND {right}";
                case AstNodeType.Constant:
                    return GetConstantValue(leaf?.Value);
                case AstNodeType.Eq:
                    return $"{AstToQueryString(branch?.Left)}:{AstToQueryString(branch?.Right)}";
                case AstNodeType.LtEq:
                    return $"{AstToQueryString(branch?.Left)}:<{AstToQueryString(branch?.Right)}";
                case AstNodeType.GtEq:
                    return $"{AstToQueryString(branch?.Left)}:>{AstToQueryString(branch?.Right)}";
                case AstNodeType.Member:
                    return leaf?.Value.ToString();
                case AstNodeType.Or:
                    return $"{AstToQueryString(branch?.Left)} OR {AstToQueryString(branch?.Right)}";
                case AstNodeType.Not:
                    return $"{AstToQueryString(unary?.Child)}:false";

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string GetConstantValue(object value)
        {
            switch (value)
            {
                case string strValue:
                    return $"'{strValue}'";
                case DateTime dtValue:
                    return $"'{GetDtString(dtValue)}'";
                case bool boolValue:
                    return boolValue.ToString().ToLower();
            }
            return value == null ? "null" : $"{value}";
        }

        private static string GetDtString(DateTime dtValue)
        {
            var utc = dtValue.ToUniversalTime();
            //var includeTime = utc.Hour != 0 || utc.Minute != 0 || utc.Second != 0;
            return $"{utc.Year:0000}-{utc.Month:00}-{utc.Day:00}";
        }

        private static FreshdeskTicketSearchAstNode Evaluate(Expression exp)
        {
            return new FreshdeskTicketSearchAstLeafNode
            {
                Type = AstNodeType.Constant,
                Value = Expression.Lambda(exp).Compile().DynamicInvoke()
            };
        }
    }
}