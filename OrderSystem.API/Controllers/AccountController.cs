using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderSystem.API.Models.Account;
using OrderSystem.API.Settings;
using OrderSystem.Core.Services;
using OrderSystem.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace OrderSystem.API.Controllers
{
    [ApiController]
    [Route("/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserAuthenticationService authenticationService;
        
        public AccountController(UserAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
        {
            var result = authenticationService.Login(request.Username, request.Password);
            if (result.Error is not null)
                return Unauthorized(result);
            return Ok(result);
        }

        [HttpPost("register")]
        public ActionResult<LoginResponse> Register([FromBody] RegisterRequest request)
        {
            var result = authenticationService.Register(request.Username, request.Password, UserRole.Public);
            if (result.Error is not null)
                return Unauthorized(result);
            return Ok(result);
        }
    }
}