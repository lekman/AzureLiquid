// <copyright file="LiquidParserException.cs">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid
// Created: 2022-10-16 07:46
// </copyright>

using System.Runtime.Serialization;

namespace AzureLiquid.Exceptions
{
    /// <summary>
    /// Thrown when a <see cref="LiquidParser"/> object is incorrectly configured during parsing.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class LiquidParserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidParserException"/> class.
        /// </summary>
        public LiquidParserException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidParserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LiquidParserException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidParserException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public LiquidParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidParserException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected LiquidParserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}