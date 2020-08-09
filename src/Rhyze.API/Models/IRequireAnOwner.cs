using Rhyze.API.Filters;
using System;

namespace Rhyze.API.Models
{
    /// <summary>
    /// Implementing this interface will have the <see cref="OwnerId"/> property set 
    /// to the authenticated user id on incoming request models. See the <see cref="BindRequestModelsToUserAttribute"/> attribute
    /// for implementation details.
    /// </summary>
    interface IRequireAnOwner
    {
        /// <summary>
        /// This property will be automatically populated by the <see cref="BindRequestModelsToUserAttribute"/>.
        /// </summary>
        Guid OwnerId { get; set; }
    }
}
