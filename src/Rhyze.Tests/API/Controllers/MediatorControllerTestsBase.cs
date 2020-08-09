using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Rhyze.Tests.Fixtures;
using System;

namespace Rhyze.Tests.API.Controllers
{
    public abstract class MediatorControllerTestsBase<T> where T : ControllerBase
    {
        protected T Controller { get; }

        protected Mock<IMediator> Mediator { get; } = new Mock<IMediator>();

        protected ClaimsPrincipalFixture PrincipalFixture { get; private set; } = new ClaimsPrincipalFixture();

        public MediatorControllerTestsBase()
        {
            var httpContext = new DefaultHttpContext() { User = PrincipalFixture.User };

            Controller = (T)Activator.CreateInstance(typeof(T), new[] { Mediator.Object });

            Controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        protected void UserHasRhyzeId()
        {
            PrincipalFixture = PrincipalFixture.WithRhyzeId();
        }
    }
}