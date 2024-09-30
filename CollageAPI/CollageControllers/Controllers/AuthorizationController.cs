using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollageControllers.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuth _AuthService;

        public AuthorizationController(IAuth AuthService)
        {
            _AuthService = AuthService;
        }

        [HttpPost("/register")]
        public async Task<ActionResult> Register([FromBody]RegisterEF Model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            AuthEF Result = await _AuthService.RegisterAsync(Model);

            if (!Result.IsAuthenticed)
                return BadRequest(Result.Message);

            return Ok(new {Token = Result.Token, ExpiredTime = Result.ExpiredTime});
        }

        [HttpPost("/login")]
        public async Task<ActionResult> Login(LoginEF Login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            AuthEF Result = await _AuthService.LoginAsync(Login);

            if (!Result.IsAuthenticed)
                return BadRequest(Result.Message);

            return Ok(Result);
        }

    }
}
