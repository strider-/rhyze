using System;

namespace Rhyze.API.Models
{
    public class AuthenticatedUser
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }
    }
}
