using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ATech.ContactFormServer.Domain.Entities
{
    /// <summary>
    /// Account Entity
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Account()
        {
            this.Messages = new List<Message>();
        }

        /// <summary>
        /// User's unique id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// User's registration email
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string EMail { get; set; }

        /// <summary>
        /// Account's messages
        /// </summary>
        public ICollection<Message> Messages { get; set; }

        /// <summary>
        /// Account's enabling flag
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Account creation Date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Item creator entity
        /// </summary>        
        public string CreatedBy { get; set; }

        /// <summary>
        /// Item editing date time
        /// </summary>
        public DateTime? Modified { get; set; }

        /// <summary>
        /// Item editor entity
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
