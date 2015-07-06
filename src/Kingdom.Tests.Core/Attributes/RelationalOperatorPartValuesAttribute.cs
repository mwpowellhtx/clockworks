using NUnit.Framework;

namespace Kingdom
{
    public class RelationalOperatorPartValuesAttribute : ValuesAttribute
    {
        public RelationalOperatorPartValuesAttribute()
            : base(
                OperatorPart.Equality
                , OperatorPart.Inequality
                , OperatorPart.GreaterThan
                , OperatorPart.LessThan
                , OperatorPart.GreaterThanOrEqual
                , OperatorPart.LessThanOrEqual
                )
        {
        }
    }
}
