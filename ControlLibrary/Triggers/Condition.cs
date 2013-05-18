namespace ControlLibrary.Triggers
{
    using Windows.UI.Xaml;

    /// <summary>
    /// A condition that can be configured on a <see cref="TriggerAction"/> that must evaluate to <c>true</c>
    /// in order for the action to be invoked.
    /// </summary>
    public class Condition : AssociatableElement
    {
        #region Static Fields

        /// <summary>
        /// The Operator dependency property of type <see cref="ConditionOperator"/>.
        /// </summary>
        public static readonly DependencyProperty OperatorProperty = DependencyProperty.Register(
            "Operator", typeof(ConditionOperator), typeof(Condition), new PropertyMetadata(ConditionOperator.Equals));

        /// <summary>
        /// The LeftOperand dependency property of type <see cref="object"/>.
        /// </summary>
        public static readonly DependencyProperty LeftOperandProperty = DependencyProperty.Register(
            "LeftOperand", 
            typeof(object), 
            typeof(Condition), 
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// The RightOperand dependency property of type <see cref="object"/>.
        /// </summary>
        public static readonly DependencyProperty RightOperandProperty = DependencyProperty.Register(
            "RightOperand", 
            typeof(object), 
            typeof(Condition), 
            new PropertyMetadata(DependencyProperty.UnsetValue));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="ConditionOperator"/> that will be used as the operator
        /// in the conditional statement.
        /// </summary>
        public ConditionOperator Operator
        {
            get
            {
                return (ConditionOperator)this.GetValue(OperatorProperty);
            }

            set
            {
                this.SetValue(OperatorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the left operand of the statement.
        /// </summary>
        public object LeftOperand
        {
            get
            {
                return this.GetValue(LeftOperandProperty);
            }

            set
            {
                this.SetValue(LeftOperandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the right operand of the statement.
        /// </summary>
        public object RightOperand
        {
            get
            {
                return this.GetValue(RightOperandProperty);
            }

            set
            {
                this.SetValue(RightOperandProperty, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Tests the configured condition and returns the result.
        /// </summary>
        /// <returns>
        /// The result of the configured condition.
        /// </returns>
        public bool EvaluateCondition()
        {
            return ValueComparer.Compare(this.LeftOperand, this.Operator, this.RightOperand);
        }

        #endregion
    }
}