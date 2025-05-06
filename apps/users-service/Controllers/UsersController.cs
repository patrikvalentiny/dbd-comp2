using Microsoft.AspNetCore.Mvc;
using UsersService.Domain.Models;
using UsersService.Services;

namespace UsersService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] User user)
    {
        var createdUser = await userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        var existingUser = await userService.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        await userService.UpdateUserAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        await userService.DeleteUserAsync(id);
        return NoContent();
    }
}
