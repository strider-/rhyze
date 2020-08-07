﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rhyze.API.Models;
using Rhyze.API.Queries;
using Rhyze.Data;
using System.Threading.Tasks;

namespace Rhyze.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDatabase _db;

        public UserController(IMediator mediator, IDatabase db)
        {
            _mediator = mediator;
            _db = db;
        }

        [Route("/me")]
        public async Task<AuthenticatedUser> MeAsync()
        {
            return await _mediator.Send(new GetAuthenticatedUserQuery(User));
        }
    }
}