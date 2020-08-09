using System;

namespace Rhyze.Core.Models
{
    /// <summary>
    /// Represents a user of the service
    /// </summary>
    public class User
    {
        /// <summary>
        /// The application specific id of the user
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The external identity id of the user
        /// </summary>
        public string IdentityId { get; set; }

        /// <summary>
        /// The email address of the user
        /// </summary>
        public string Email { get; set; }
    }
}
