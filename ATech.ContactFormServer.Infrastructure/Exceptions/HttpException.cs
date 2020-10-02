using System;

namespace ATech.ContactFormServer.Infrastructure.Exceptions
{
    /// <summary>
    /// Http Exception
    /// </summary>
    public class HttpException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="content"></param>
        public HttpException(int statusCode, string content)
            : base(content)
        {
            StatusCode = statusCode;
            Content = content;
        }

        /// <summary>
        /// HttpStatus Code
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Exception's message
        /// </summary>
        public string Content { get; }
    }
}
