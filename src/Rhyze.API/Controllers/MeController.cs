using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rhyze.API.Models;
using Rhyze.API.Queries;
using System.Threading.Tasks;

namespace Rhyze.API.Controllers
{
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MeController(IMediator mediator) => _mediator = mediator;

        [Route("/me")]
        public async Task<AuthenticatedUser> MeAsync()
        {
            return await _mediator.Send(new GetAuthenticatedUserQuery(User));
        }
    }
}