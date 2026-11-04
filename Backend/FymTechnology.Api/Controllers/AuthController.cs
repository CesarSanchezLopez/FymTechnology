using FluentValidation;
using FymTechnology.Application.DTOs.Auth;
using FymTechnology.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FymTechnology.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<LoginRequest> _loginValidator;

    public AuthController(
        IAuthService authService,
        IValidator<LoginRequest> loginValidator)
    {
        _authService = authService;
        _loginValidator = loginValidator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(x => new
            {
                x.PropertyName,
                x.ErrorMessage
            }));
        }

        var response = await _authService.LoginAsync(request);

        if (response is null)
            return Unauthorized("Credenciales inválidas.");

        return Ok(response);
    }
}