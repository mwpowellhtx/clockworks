namespace Kingdom.Clockworks
{
    internal enum OperatorPart
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
    }
}
