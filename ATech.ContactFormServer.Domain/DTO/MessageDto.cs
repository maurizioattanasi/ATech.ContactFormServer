using System;

namespace ATech.ContactFormServer.Domain.DTO
{
    /// <summary>
    /// Message model
    /// </summary>
    public class MessageDto
    {
        /// <summary>
        /// Message sender Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sender email address
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// Message content
        /// </summary>
        public string Message { get; set; }
    }
}
