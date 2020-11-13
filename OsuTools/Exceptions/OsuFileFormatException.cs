using System;

namespace OsuTools.Exceptions {
    public class OsuFileFormatException : Exception {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the System.Exception class
        /// </summary>
        public OsuFileFormatException() { }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified error message 
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="sectionName">The name of the section that produced the error/param>
        public OsuFileFormatException(string message, string sectionName) :
            base($"{message} Section: {sectionName}") { _sectionName = sectionName; }

        /// <summary>
        /// Initializes a new instance of the System.Exception class
        /// with a specified error message and a reference to 
        /// the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="sectionName">The name of the section that produced the error/param>
        /// <param name="inner">The exception that is the cause of the current exception</param>
        public OsuFileFormatException(string message, string sectionName, Exception inner) :
            base($"{message} Section: {sectionName}", inner) { _sectionName = sectionName; }
        #endregion
        
        private readonly string _sectionName;
    }
}
