using System;

namespace ATech.ContactFormServer.Api.DTO
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
        /// Sender Phone Number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Message content
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Honeypot check for SPAM BOTS protection
        /// </summary>
        public string Honeypot { get; set; }
    }
}
