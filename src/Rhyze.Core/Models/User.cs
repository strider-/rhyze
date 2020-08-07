using System;

namespace Rhyze.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string IdentityId { get; set; }

        public string Email { get; set; }
    }
}
