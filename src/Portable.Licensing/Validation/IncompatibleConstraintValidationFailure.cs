namespace Portable.Licensing.Validation
{
    /// <summary>
    /// Represents a failure when the <see cref="License.Constraint"/> is incompatible.
    /// </summary>
    public class IncompatibleConstraintValidationFailure : IValidationFailure
    {
        /// <summary>
        /// Gets or sets a message that describes the validation failure.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a message that describes how to recover from the validation failure.
        /// </summary>
        public string HowToResolve { get; set; }
    }
}