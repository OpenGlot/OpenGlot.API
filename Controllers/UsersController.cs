﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyglotAPI.Data;
using System.Security.Claims;

namespace PolyglotAPI.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;
    using PolyglotAPI.Data.Models;
    using PolyglotAPI.Data.Repos;
    using PolyglotAPI.Common;

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetUsers()
        {
            _logger.LogInformation("Getting all users");
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfile>> GetUser(Guid id)
        {
            _logger.LogInformation($"Getting user with ID: {id}");
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning($"User with ID: {id} not found");
                return NotFound();
            }

            return Ok(user);
        }
        
        [HttpPut]
        public async Task<ActionResult<UserProfile>> AddUser(UserProfile user)
        {
            _logger.LogInformation("Adding a new user");
            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserProfile user)
        {
            if (id != user.UserId)
            {
                _logger.LogWarning($"User ID mismatch: {id} != {user.UserId}");
                return BadRequest();
            }

            // Check if users is 18 years or older
            if (user.DateOfBirth.Date.AddYears(18) > DateTime.Today.Date)
            {
                return BadRequest("User must be 18 years or older");
            }
            // Check if user is updating their own profile
            if (user.UserId != User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).ToGuid())
            {
                return Unauthorized();
            }
            // Check if user's target language is different from their native language
            if (user.NativeLanguage == user.TargetLanguage ||
                user.NativeLanguage == user.TargetLanguage2 ||
                user.NativeLanguage == user.TargetLanguage3)
            {
                return BadRequest("Target language must be different from native language");
            }

            _logger.LogInformation($"Updating user with ID: {id}");
           

            try
            {
                await _userRepository.UpdateUserAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! (await _userRepository.GetUserByIdAsync(id) != null))
                {
                    _logger.LogWarning($"User with ID: {id} not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            _logger.LogInformation($"Deleting user with ID: {id}");
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID: {id} not found");
                return NotFound();
            }

            await _userRepository.DeleteUserAsync(id);

            return NoContent();
        }

        

        // POST: api/Users/5/Roles
        [HttpPost("{id}/Roles")]
        public async Task<IActionResult> UpdateUserRole(Guid id, UserRole userRole)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.UserRole = userRole;

            await _userRepository.UpdateUserAsync(user);

            return NoContent();
        }

        // GET: api/Users/{id}/Profile
        [HttpGet("{id}/Profile")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(Guid id)
        {
            _logger.LogInformation($"Getting profile for user with ID: {id}");
            var userProfile = await _userRepository.GetUserProfileAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }

        // GET: api/Users/{id}/Badges
        [HttpGet("{id}/Badges")]
        public async Task<ActionResult<IEnumerable<Badge>>> GetUserBadges(Guid id)
        {
            _logger.LogInformation($"Getting badges for user with ID: {id}");
            var badges = await _userRepository.GetUserBadgesAsync(id);
            return Ok(badges);
        }

        // GET: api/Users/{id}/Notifications
        [HttpGet("{id}/Notifications")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetUserNotifications(Guid id)
        {
            _logger.LogInformation($"Getting notifications for user with ID: {id}");
            var notifications = await _userRepository.GetUserNotificationsAsync(id);
            return Ok(notifications);
        }

        // GET: api/Users/{id}/Flashcards
        [HttpGet("{id}/Flashcards")]
        public async Task<ActionResult<IEnumerable<Flashcard>>> GetUserFlashcards(Guid id)
        {
            _logger.LogInformation($"Getting flashcards for user with ID: {id}");
            var flashcards = await _userRepository.GetUserFlashcardsAsync(id);
            return Ok(flashcards);
        }

        // GET: api/Users/{id}/Progresses
        [HttpGet("{id}/Progresses")]
        public async Task<ActionResult<IEnumerable<Progress>>> GetUserProgresses(Guid id)
        {
            _logger.LogInformation($"Getting progresses for user with ID: {id}");
            var progresses = await _userRepository.GetUserProgressesAsync(id);
            return Ok(progresses);
        }
    }
}
