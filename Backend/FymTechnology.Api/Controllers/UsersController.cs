using FluentValidation;
using FymTechnology.Application.DTOs.Users;
using FymTechnology.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FymTechnology.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SuperAdmin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserRequest> _validator;

    public UsersController(
        IUserService userService,
        IValidator<CreateUserRequest> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(x => new
            {
                x.PropertyName,
                x.ErrorMessage
            }));
        }

        var userId = await _userService.CreateAsync(request);

        return Created(string.Empty, new { Id = userId });
    }
    [HttpGet]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }
    [HttpPut("{userId:guid}/roles/{roleId:guid}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> AssignRole(Guid userId, Guid roleId)
    {
        await _userService.AssignRoleAsync(userId, roleId);

        return NoContent();
    }
}