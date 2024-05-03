using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OrganizeApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : Controller
    {
        private ISender _mediator;
        public ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
    }
}
