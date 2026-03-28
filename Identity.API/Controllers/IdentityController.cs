using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventBus;
using IdentityService.Data.Entities;
using IdentityService.Models;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IConfiguration _config;

    public IdentityController(
        UserManager<ApplicationUser> userManager,
        IPublishEndpoint publishEndpoint,
        IConfiguration config)
    {
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel requestModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponseModel
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Failed",
                Errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>())
            });
        }

        var user = new ApplicationUser
        {
            UserName = requestModel.Email,
            Email = requestModel.Email,
            FirstName = requestModel.FirstName,
            LastName = requestModel.LastName,
            PhoneNumber = requestModel.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, requestModel.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new ErrorResponseModel
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Registration Failed",
                Errors = result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description })
            });
        }

        await _publishEndpoint.Publish<UserRegisteredEvent>(new UserRegisteredEvent
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber
        });
            
        return Ok(new RegisterResponseModel()
        {
            Id = user.Id
        });

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel requestModel)
    {
        var user = await _userManager.FindByEmailAsync(requestModel.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, requestModel.Password))
            return Unauthorized(new ErrorResponseModel
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Invalid credentials"
            });
        var token = GenerateJwtToken(user);
        
        return Ok(new LoginResponseModel()
        {
            Token =  token
        });
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}