namespace Kingdom
{
    public enum OperatorPart
    {
        /// <summary>
        /// 
        /// </summary>
        Increment,

        /// <summary>
        /// 
        /// </summary>
        Decrement,

        /// <summary>
        /// 
        /// </summary>
        Addition,

        /// <summary>
        /// 
        /// </summary>
        Subtraction,

        //TODO: C# just "handles" this part, translating "a += b" into "a = a + b" auto-magically.
        ///// <summary>
        ///// 
        ///// </summary>
        //Assignment,

        /// <summary>
        /// 
        /// </summary>
        Multiplication,

        /// <summary>
        /// 
        /// </summary>
        Division,

        /// <summary>
        /// 
        /// </summary>
        GreaterThan,

        /// <summary>
        /// 
        /// </summary>
        LessThan,

        /// <summary>
        /// 
        /// </summary>
        GreaterThanOrEqualTo,

        /// <summary>
        /// 
        /// </summary>
        LessThanOrEqualTo,

        /// <summary>
        /// 
        /// </summary>
        Equality,

        /// <summary>
        /// 
        /// </summary>
        Inequality,
    }
}
