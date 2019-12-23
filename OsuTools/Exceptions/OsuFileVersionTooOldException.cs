using System;

namespace OsuTools.Exceptions {
    public class OsuFileVersionTooOldException : Exception {

        /// <summary>
        /// Initializes a new instance of the System.Exception class
        /// </summary>
        public OsuFileVersionTooOldException() { }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified error message 
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public OsuFileVersionTooOldException(string message) :
            base(message) { }

        /// <summary>
        /// Initializes a new instance of the System.Exception class
        /// with a specified error message and a reference to 
        /// the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="inner">The exception that is the cause of the current exception</param>
        public OsuFileVersionTooOldException(string message, Exception inner) :
            base(message, inner) { }
    }
}
