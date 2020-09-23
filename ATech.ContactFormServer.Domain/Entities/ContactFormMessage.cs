using System;
using System.ComponentModel.DataAnnotations;

namespace ATech.ContactFormServer.Domain.Entities
{
    /// <summary>
    /// A single Contact Form Message Item
    /// </summary>
    public class ContactFormMessage
    {
        /// <summary>
        /// Item Unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Message sender Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Sender email address
        /// </summary>
        [Required]
        public string EMail { get; set; }

        /// <summary>
        /// Sender Phone Number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Message content
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Message creation time
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Message creator entity (tipically the id of the account)
        /// </summary>
        /// <value></value>
        public string CreatedBy { get; set; }
    }
}
