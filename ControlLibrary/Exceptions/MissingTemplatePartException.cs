using System;

namespace ControlLibrary.Exceptions
{
    /// <summary>
    /// An exception of this type is thrown when an ControlTemplate with missing crucial parts is applied.
    /// </summary>
    public class MissingTemplatePartException : Exception
    {
        private Type type;
        
        /// <summary>
        /// Initializes a new instance of the MissingTemplatePartException class.
        /// </summary>
        /// <param name="missingPartType">The type of the missing part.</param>
        /// <param name="name">The name of the missing part.</param>
        public MissingTemplatePartException(Type missingPartType, string name)
            : base(name)
        {
            this.type = missingPartType;
        }

        /// <summary>
        /// Gets the message of the exception.
        /// </summary>
        public override string Message
        {
            get
            {
                return base.Message + " - " + this.type.Name;
            }
        }
    }
}
