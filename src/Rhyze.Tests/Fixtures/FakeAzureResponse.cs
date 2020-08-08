using Azure;
using System;

namespace Rhyze.Tests.Fixtures
{
    internal class FakeAzureResponse<T> : Response<T>
    {
        public FakeAzureResponse(T val) => Value = val;

        public override T Value { get; }

        public override Response GetRawResponse()
        {
            throw new NotImplementedException();
        }
    }
}