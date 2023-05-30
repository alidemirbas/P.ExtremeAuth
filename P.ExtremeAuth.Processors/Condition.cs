using System.Linq.Expressions;
using P.ExtremeAuth.Entity.Enums;

namespace P.ExtremeAuth.Processors
{
    //todo daha iyi cozum
    public class Condition
    {
        public static bool Compile(string left, string right, TypeCode typeCode, ConditionOperator op)
        {
            Type type = Type.GetType($"System.{typeCode}");
            ParameterExpression paramL = Expression.Parameter(type, "left");
            ParameterExpression paramR = Expression.Parameter(type, "right");
            BinaryExpression logic;

            switch (op)
            {
                //todo oplar eklenecek
                case ConditionOperator.Equals: logic = Expression.Equal(paramL, paramR); break;
                case ConditionOperator.NotEquals: logic = Expression.NotEqual(paramL, paramR); break;
                case ConditionOperator.LessThan: logic = Expression.LessThan(paramL, paramR); break;
                case ConditionOperator.LessThanOrEqual: logic = Expression.LessThanOrEqual(paramL, paramR); break;
                case ConditionOperator.GreaterThan: logic = Expression.GreaterThan(paramL, paramR); break;
                case ConditionOperator.GreaterThanOrEqual: logic = Expression.GreaterThanOrEqual(paramL, paramR); break;

                default: throw new ArgumentException("Invalid comparison operator: {0}", op.ToString());
            }

            Delegate delg = Expression.Lambda(logic, paramL, paramR).Compile();//.Lambda<Func<T, T, bool>>(this.Logic, paramL, paramR).Compile();

            object l, r;
            l = Convert.ChangeType(left, typeCode);
            r = Convert.ChangeType(right, typeCode);

            return (bool)delg.DynamicInvoke(l, r);
        }
    }
}
