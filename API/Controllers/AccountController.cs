using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly TokenServices tokenServices;
        public AccountController(UserManager<AppUser> userManager, TokenServices tokenServices)
        {
            this.tokenServices = tokenServices;
            this.userManager = userManager;
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await this.userManager.FindByEmailAsync(loginDto.Email);

            if(user == null) return Unauthorized();

            var result = await this.userManager.CheckPasswordAsync(user, loginDto.Password);

            if(result)
            {
                return new UserDto
                {
                    DisplayName = user.DisplayName,
                    Image = null,
                    Token = tokenServices.CreateToken(user),
                    UserName = user.UserName
                };
            }

            return Unauthorized();
        } 
    }
}