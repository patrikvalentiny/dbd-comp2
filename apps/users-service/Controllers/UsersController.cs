using Microsoft.AspNetCore.Mvc;
using UsersService.Domain.Models;
using UsersService.Services;

namespace UsersService.Controllers;

/// <summary>
/// API controller for managing users in the second-hand e-commerce platform.
/// Provides endpoints for creating, retrieving, updating, and deleting user accounts.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Retrieves all users registered in the system.
    /// </summary>
    /// <returns>A collection of all user accounts with their details.</returns>
    /// <response code="200">Returns the list of users</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointName("GetAllUsers")]
    [EndpointSummary("Retrieves all users in the system.")]
    [EndpointDescription("Returns a collection of all user accounts with their profile details.")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Retrieves a specific user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>The user with the specified ID.</returns>
    /// <response code="200">Returns the requested user</response>
    /// <response code="404">If the user with the specified ID doesn't exist</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("GetUserById")]
    [EndpointSummary("Retrieves a specific user by ID.")]
    [EndpointDescription("Fetches the complete details of a single user specified by their unique identifier.")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    /// <summary>
    /// Creates a new user account in the system.
    /// </summary>
    /// <param name="user">The user information to create.</param>
    /// <returns>The newly created user with their generated ID.</returns>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">If the user data is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointName("CreateUser")]
    [EndpointSummary("Creates a new user in the system.")]
    [EndpointDescription("Creates a new user account. The user information must be provided in the request body.")]
    public async Task<ActionResult<User>> CreateUser([FromBody] User user)
    {
        var createdUser = await userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
    }

    /// <summary>
    /// Updates an existing user's information.
    /// </summary>
    /// <param name="id">The unique identifier of the user to update.</param>
    /// <param name="user">The updated user information.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">If the user was successfully updated</response>
    /// <response code="400">If the user ID in the URL doesn't match the ID in the request body</response>
    /// <response code="404">If the user with the specified ID doesn't exist</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("UpdateUser")]
    [EndpointSummary("Updates an existing user.")]
    [EndpointDescription("Updates the details of an existing user account including profile information, contact details, etc.")]
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

    /// <summary>
    /// Deletes a user from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">If the user was successfully deleted</response>
    /// <response code="404">If the user with the specified ID doesn't exist</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("DeleteUser")]
    [EndpointSummary("Removes a user from the system.")]
    [EndpointDescription("Permanently deletes a user account from the database. This operation cannot be undone.")]
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
