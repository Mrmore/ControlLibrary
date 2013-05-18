namespace ControlLibrary.Triggers
{
    using System;

    /// <summary>
    /// The value comparer helper code.
    /// </summary>
    internal static class ValueComparer
    {
        #region Methods

        /// <summary>
        /// Compares a left and right operand with respect to a given operator.
        /// </summary>
        /// <param name="leftOperand">
        /// The left operand.
        /// </param>
        /// <param name="conditionOperator">
        /// The condition operator.
        /// </param>
        /// <param name="rightOperand">
        /// The right operand.
        /// </param>
        /// <returns>
        /// <c>true</c> if the result of comparing the two values is <c>true</c> with respect to the 
        /// operator, otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the two operators can't be compared.
        /// </exception>
        internal static bool Compare(object leftOperand, ConditionOperator conditionOperator, object rightOperand)
        {
            if (leftOperand != null && rightOperand != null && leftOperand.GetType() != rightOperand.GetType())
            {
                try
                {
                    leftOperand = Convert.ChangeType(leftOperand, rightOperand.GetType());
                }
                catch (Exception ex)
                {
                    if (ex is FormatException || ex is InvalidCastException)
                    {
                        // If the conversion fails, then the types aren't equal so this test will only
                        // pass if the operator is NotEquals
                        return conditionOperator == ConditionOperator.NotEquals;
                    }

                    throw;
                }
            }

            var comparableLeft = leftOperand as IComparable;
            var comparableRight = rightOperand as IComparable;
            if (comparableLeft != null && comparableRight != null)
            {
                int result = comparableLeft.CompareTo(comparableRight);
                switch (conditionOperator)
                {
                    case ConditionOperator.Equals:
                        return result == 0;
                    case ConditionOperator.NotEquals:
                        return result != 0;
                    case ConditionOperator.GreaterThan:
                        return result > 0;
                    case ConditionOperator.LessThan:
                        return result < 0;
                    case ConditionOperator.GreaterThanOrEqual:
                        return result >= 0;
                    case ConditionOperator.LessThanOrEqual:
                        return result <= 0;
                    default:
                        throw new InvalidOperationException("Unknown operator " + conditionOperator);
                }
            }

            switch (conditionOperator)
            {
                case ConditionOperator.Equals:
                    return object.Equals(leftOperand, rightOperand);
                case ConditionOperator.NotEquals:
                    return !object.Equals(leftOperand, rightOperand);
                default:
                    throw new InvalidOperationException(
                        "Unable to compare operands - when using operands other than Equals and NotEquals you must use types that implement IComparable and values that are not null.");
            }
        }

        #endregion
    }
}