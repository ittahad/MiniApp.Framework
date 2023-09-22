using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MiniApp.PgSQL
{
    public static class SqlExpressionHelper
    {
        public static string GetUpdateSetClause<T>(T updateObj)
        {
            if (updateObj == null)
            {
                throw new ArgumentNullException(nameof(updateObj));
            }

            var properties = typeof(T).GetProperties();
            var setClauseBuilder = new StringBuilder();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(updateObj);

                // Exclude properties with null values
                if (propertyValue != null)
                {
                    if (setClauseBuilder.Length > 0)
                    {
                        setClauseBuilder.Append(", ");
                    }

                    setClauseBuilder.Append($"{propertyName} = {ParseValue(updateObj, property)}");
                }
            }

            if (setClauseBuilder.Length == 0)
            {
                throw new ArgumentException("No valid properties found for update.");
            }

            return setClauseBuilder.ToString();
        }

        public static object GetWhereClause<T>(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return Visit(expression.Body, "");
        }

        private static object Visit(Expression expression, string direction)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                var left = Visit(binaryExpression.Left, "LEFT");
                var right = Visit(binaryExpression.Right, "RIGHT");
                var operation = GetSqlOperator(binaryExpression.NodeType);

                return $"({left} {operation} {right})";
            }
            else if (expression is MemberExpression memberExpression)
            {
                if (direction == "RIGHT")
                {
                    var val = GetValue(memberExpression);
                    return ParseValue(val.ToString()!, val.GetType())!;
                }
                else {
                    return memberExpression.Member.Name;
                }

            }
            else if (expression is ConstantExpression constantExpression)
            {
                var value = constantExpression.Value;
                if (value == null)
                {
                    return "NULL";
                }

                var type = value.GetType();
                if (type.IsNumericType())
                {
                    return value.ToString()!;
                }
                else if (type == typeof(string))
                {
                    return $"'{value}'";
                }
                else
                {
                    throw new NotSupportedException($"Constant expression of type {type} is not supported.");
                }
            }

            throw new NotSupportedException($"Expression of type {expression.GetType()} is not supported.");
        }
        private static object GetValue(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));

            var getterLambda = Expression.Lambda<Func<object>>(objectMember);

            var getter = getterLambda.Compile();

            return getter();
        }

        private static string GetSqlOperator(ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.OrElse:
                    return "OR";
                default:
                    throw new NotSupportedException($"Binary operator {nodeType} is not supported.");
            }
        }

        public static bool IsNumericType(this Type type)
        {
            return type == typeof(int) || type == typeof(double) || type == typeof(decimal)
                || type == typeof(float) || type == typeof(long) || type == typeof(short)
                || type == typeof(uint) || type == typeof(ulong) || type == typeof(ushort)
                || type == typeof(byte) || type == typeof(sbyte);
        }

        public static object? ParseValue<T>(T item, PropertyInfo prop)
        {
            if (item == null) throw new InvalidDataException();

            var val = prop.GetValue(item);
            var srcType = val!.GetType();

            return MatchType(val, srcType);
        }

        public static object? ParseValue(object item, Type type)
        {
            if (item == null) throw new InvalidDataException();

            var srcType = type;

            return MatchType(item, srcType);
        }

        private static object? MatchType(object item, Type srcType)
        {
            if (srcType == typeof(string))
            {
                return $"'{item}'";
            }
            else if (srcType == typeof(int))
            {
                return (int)item!;
            }
            else if (srcType == typeof(double))
            {
                return (double)item!;
            }
            else if (srcType == typeof(float))
            {
                return (float)item!;
            }
            else if (srcType == typeof(decimal))
            {
                return (decimal)item!;
            }
            else if (srcType == typeof(bool))
            {
                return (bool)item!;
            }
            else if (srcType == typeof(DateTime))
            {
                return (DateTime)item!;
            }

            return item!.ToString();
        }
    }
}
