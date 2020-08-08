using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;

namespace Rhyze.Tests.Fixtures
{
    public static class MediatorControllerGenerator
    {
        public static T Create<T>(ClaimsPrincipalFixture fixture, Mock<IMediator> mock) where T : ControllerBase
        {
            var httpContext = new DefaultHttpContext() { User = fixture.User };
            var controller = (T)Activator.CreateInstance(typeof(T), new[] { mock.Object });

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            return controller;
        }
    }
}
