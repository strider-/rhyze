using System;
using System.Security.Claims;

namespace Rhyze.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Returns the identity provider id for this user.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string IdentityId(this ClaimsPrincipal principal) => principal.FindFirstValue("user_id");

        /// <summary>
        /// Returns the user email address.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string Email(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.Email);

        /// <summary>
        /// Returns the application identifier for this user.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Guid UserId(this ClaimsPrincipal principal) => Guid.Parse(principal.FindFirstValue("rhyze_id"));
    }
}