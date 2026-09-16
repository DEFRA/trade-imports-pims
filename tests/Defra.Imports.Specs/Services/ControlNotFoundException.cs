namespace Defra.Imports.Specs.Services
{
    using System;

    /// <summary>
    /// The exception that is thrown when a form does not contain a control that a test expected.
    /// </summary>
    /// <remarks>
    /// This is deliberately distinct from a general <see cref="InvalidOperationException"/> so that
    /// tests can tell the difference between a control that the solution does not implement, which
    /// is reportable as a known defect, and a genuine failure to interact with a control that does
    /// exist.
    /// </remarks>
    [Serializable]
    public class ControlNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlNotFoundException"/> class.
        /// </summary>
        public ControlNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ControlNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of this exception.</param>
        public ControlNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The streaming context.</param>
        protected ControlNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
