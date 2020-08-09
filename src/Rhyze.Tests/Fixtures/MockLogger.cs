using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace Rhyze.Tests.Fixtures
{
    public class MockLogger : Mock<ILogger>
    {
        public void Verify(LogLevel level, string message)
        {
            Func<object, Type, bool> state = (v, t) => v.ToString().Contains(message);

            Verify(l =>
                l.Log(
                    It.Is<LogLevel>(l => l == level),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)
                )
            );
        }
    }
}
