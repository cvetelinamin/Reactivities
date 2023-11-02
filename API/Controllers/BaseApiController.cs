using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {     
        private IMediator mediator;

        protected IMediator Mediator => this.mediator ??=
             HttpContext.RequestServices.GetService<IMediator>();

    }
}