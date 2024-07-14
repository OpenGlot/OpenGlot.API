using PolyglotAPI.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PolyglotAPI.Data.Repos
{


    public interface IUserRepository
    {
        Task<IEnumerable<UserProfile>> GetAllUsersAsync();
        Task<UserProfile> GetUserByIdAsync(Guid userId);
        Task AddUserAsync(UserProfile user);
        Task UpdateUserAsync(UserProfile user);
        Task DeleteUserAsync(Guid userId);

        Task<UserProfile?> GetUserProfileAsync(Guid userId);
        Task<IEnumerable<Badge>> GetUserBadgesAsync(Guid userId);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId);
        Task<IEnumerable<Flashcard>> GetUserFlashcardsAsync(Guid userId);
        Task<IEnumerable<Progress>> GetUserProgressesAsync(Guid userId);

        Task AddBadgeAsync(Badge badge);
        Task AddNotificationAsync(Notification notification);
        Task AddFlashcardAsync(Flashcard flashcard);
        Task AddProgressAsync(Progress progress);

        Task<UserProfile?> GetDetailedUserProfileAsync(Guid userId);
    }



public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserProfile>> GetAllUsersAsync()
        {
            return await _context.UserProfiles.ToListAsync();
        }

        public async Task<UserProfile?> GetUserByIdAsync(Guid userId)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task AddUserAsync(UserProfile user)
        {            
            await _context.UserProfiles.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(UserProfile user)
        {
            _context.UserProfiles.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                _context.UserProfiles.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserProfile?> GetUserProfileAsync(Guid userId)
        {
            return await _context.UserProfiles
                                 .Include(u => u.Badges)
                                 .Include(u => u.Notifications)
                                 .Include(u => u.Flashcards)
                                 .Include(u => u.Progresses)
                                 .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<IEnumerable<Badge>> GetUserBadgesAsync(Guid userId)
        {
            return await _context.Badges.Where(b => b.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId)
        {
            return await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Flashcard>> GetUserFlashcardsAsync(Guid userId)
        {
            return await _context.Flashcards.Where(f => f.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Progress>> GetUserProgressesAsync(Guid userId)
        {
            return await _context.Progresses.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task AddBadgeAsync(Badge badge)
        {
            await _context.Badges.AddAsync(badge);
            await _context.SaveChangesAsync();
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task AddFlashcardAsync(Flashcard flashcard)
        {
            await _context.Flashcards.AddAsync(flashcard);
            await _context.SaveChangesAsync();
        }

        public async Task AddProgressAsync(Progress progress)
        {
            await _context.Progresses.AddAsync(progress);
            await _context.SaveChangesAsync();
        }

        // Detailed User Profile
        public async Task<UserProfile?> GetDetailedUserProfileAsync(Guid userId)
        {
            return await _context.UserProfiles
                                 .Include(u => u.Badges)
                                 .Include(u => u.Notifications)
                                 .Include(u => u.Flashcards)
                                 .Include(u => u.Progresses)
                                 .FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }

}
