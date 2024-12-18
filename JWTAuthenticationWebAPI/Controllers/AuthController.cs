using JWTAuthenticationWebAPI.Core.DTOs;
using JWTAuthenticationWebAPI.Core.Interfaces;
using JWTAuthenticationWebAPI.Core.OtherObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthenticationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Route For Seeding my roles to DB
        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            bool isOwnerRolesExists = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isAdminRolesExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isUserRolesExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

            if(isOwnerRolesExists && isAdminRolesExists && isUserRolesExists)
            {
                return Ok("Roles Seeding is Already Done!");
            }

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));

            return Ok("Role Seeding Done Successfully!");
        }

        //Route -> Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var isExistsUser = await _userManager.FindByNameAsync(registerDTO.UserName);
            if(isExistsUser != null)
            {
                return BadRequest("UserName Already Exists");
            }

            var newUser = new IdentityUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerDTO.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Because: ";
                foreach(var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return BadRequest(errorString);
            }
            //Add a Default USER Role for all user
            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);
            return Ok("User Created Successfully!");
        }

        //Route -> Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.UserName);
            if(user is null)
            {
                return Unauthorized("Invalid Credentials!");
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!isPasswordCorrect)
            {
                return Unauthorized("Invalid Credentials!");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.UserName ?? ""),
                new(ClaimTypes.NameIdentifier, user.Id),
                new("JWTID", Guid.NewGuid().ToString()),
            };

            foreach(var userRole in userRoles)
            {
                authClaims.Add(new(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);
            return Ok(token);
        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var jwtSecret = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? "");
            var authSecret = new SymmetricSecurityKey(jwtSecret);
            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return token;
        }
    }
}
