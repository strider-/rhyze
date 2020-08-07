using System.Collections.Generic;

namespace Rhyze.Core.Models
{
    public class Error
    {
        public Error(params string[] messages) => Messages = messages;

        public IEnumerable<string> Messages { get; }

        public override string ToString() => string.Join(", ", Messages);
    }
}