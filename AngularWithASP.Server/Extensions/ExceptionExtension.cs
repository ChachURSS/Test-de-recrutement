namespace System
{
    /// <summary>
    /// Extension methods for <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Gets the full stack trace of the exception.
        /// </summary>
        /// <param name="exc">Root exception</param>
        /// <returns>Full stack of the exception</returns>
        public static string GetFullStack(this Exception exc)
        {
            var message = exc.Message;
            if (exc.InnerException != null)
            {
                message += " -> " + exc.InnerException.GetFullStack();
            }
            return message;
        }
    }
}
