
using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Models;
using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo userRepo;

        public UserController(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }
        [HttpPost]
        public async Task<ActionResult<Response>> RegisterUser(RegistrationModel registrationModel)
        {
            var result = await userRepo.RegisterUserAsync(registrationModel);
            if(result.Success)
                return Ok(new Response() { Success = true, Message = result.Message });

            return BadRequest(new Response() { Message = result.Message, Success = false});
        }
        [HttpPost("login")]
        public async Task<ActionResult<Response>> LoginUser(LoginModel loginModel)
        {
            var result = await userRepo.LoginUserAsync(loginModel);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
