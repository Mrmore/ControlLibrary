namespace ControlLibrary.Triggers
{
    /// <summary>
    /// The various conditions that can be applied to an <see cref="Condition"/>
    /// </summary>
    public enum ConditionOperator
    {
        /// <summary>
        /// The left and right operands must be equal.
        /// </summary>
        Equals,

        /// <summary>
        /// The left and right operands must not be equal.
        /// </summary>
        NotEquals,

        /// <summary>
        /// The left operand must be greater than the right operand.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// The left operand must be less than the right operand.
        /// </summary>
        LessThan,

        /// <summary>
        /// The left operand must be greater than or equal to the right operand.
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// The left operand must be less than or equal to the right operand.
        /// </summary>
        LessThanOrEqual
    }
}