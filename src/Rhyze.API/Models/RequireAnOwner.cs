using Microsoft.AspNetCore.Mvc.ModelBinding;
using Rhyze.API.Filters;
using System;

namespace Rhyze.API.Models
{
    /// <summary>
    /// Inheriting from this class will give you a property that will be set to the
    /// user id of the current authenticated user, after model binding has occurred.
    /// </summary>
    public abstract class RequireAnOwner
    {
        /// <summary>
        /// This property will be automatically populated by the <see cref="BindRequestModelsToUserAttribute"/>, and
        /// cannot be bound by model binding.
        /// </summary>
        [BindNever]
        public Guid OwnerId { get; set; }
    }
}
