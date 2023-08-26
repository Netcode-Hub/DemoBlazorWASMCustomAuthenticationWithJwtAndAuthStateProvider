
using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Models;
using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo userRepo;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public UserController(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }
        [HttpPost("register")]
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

        [HttpGet("User/WF")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
