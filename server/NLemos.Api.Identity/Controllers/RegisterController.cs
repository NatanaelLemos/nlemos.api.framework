using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLemos.Api.Framework.Exceptions;
using NLemos.Api.Framework.Extensions.Controllers;
using NLemos.Api.Identity.Dto;
using NLemos.Api.Identity.Entities;
using NLemos.Api.Identity.Services;

namespace NLemos.Api.Identity.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRegisterService _service;

        private RegisterOut Map(User user) => _mapper.Map<User, RegisterOut>(user);

        private User Map(RegisterIn register) => _mapper.Map<RegisterIn, User>(register);

        public RegisterController(IMapper mapper, IRegisterService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<RegisterOut> Get()
        {
            var userEmail = this.GetUserEmail();
            var user = await _service.GetUserByEmail(userEmail);
            return Map(user);
        }

        [HttpPost]
        public async Task<RegisterOut> Post([FromBody] RegisterIn model)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(ModelState);
            }

            var user = Map(model);
            user = await _service.RegisterUser(user);
            return Map(user);
        }
    }
}
