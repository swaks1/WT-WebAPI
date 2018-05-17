using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WT_WebAPI.Entities;
using WT_WebAPI.Entities.DBContext;
using WT_WebAPI.Entities.DTO;
using WT_WebAPI.Repository.Interfaces;

namespace WT_WebAPI.Repository
{
    public class CommonRepository : ICommonRepository
    {
        WorkoutTrackingDBContext _context;

        public CommonRepository(WorkoutTrackingDBContext dbContext)
        {
            _context = dbContext;
        }


        #region Users
            public async Task<WTUser> GetUserByUsername(string username)
            {
                return await _context.Users
                                .Where(user => user.Username == username)
                                .AsNoTracking()
                                .SingleOrDefaultAsync();
            }

            public async Task<WTUser> GetUserByUsernameFullInfo(string username)
            {
                return await _context.Users
                                    .Include(w => w.Exercises)
                                        .ThenInclude(e => e.Attributes)
                                    .Include(w => w.Exercises)
                                        .ThenInclude(e => e.ExerciseRoutineEntries)
                                    .Include(w => w.WorkoutRoutines)
                                        .ThenInclude(e => e.ExerciseRoutineEntries)
                                    .Include(w => w.WorkoutPrograms)
                                        .ThenInclude(e => e.RoutineProgramEntries)
                                    .Include(w => w.WorkoutSession)
                                        .ThenInclude(s => s.ExerciseSessionEntries)
                                    .Include(w => w.BodyStatistics)
                                        .ThenInclude(s => s.BodyStatAttributes)
                                    .Include(w => w.BodyStatistics)
                                        .ThenInclude(s => s.ProgressImages)
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(user => user.Username == username);
            }

            public async Task<bool> UpdateUser(WTUserDTO user)
            {
                //_context.Entry(wTUser).State = EntityState.Modified;
                var entity = await _context.Users
                                .Where(u => u.Username == user.Username)
                                .SingleOrDefaultAsync();

                if (entity == null)
                {
                    return false;
                }

                entity.FirstName = user.FirstName;
                entity.Email = user.Email;
                entity.LastName = user.LastName;
                entity.NotificationActivated = user.NotificationActivated;

                try
                {
                    int x = await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await UserExists(entity.Username)))
                    {
                        return false;
                    }
                    else
                    {
                        throw;
                    }
                }

                return true;
            }

            public async Task<bool> DeleteUser(string username)
            {
                var wTUser = await _context.Users.SingleOrDefaultAsync(m => m.Username == username);
                if (wTUser == null)
                {
                    return false;
                }

                //_context.Users.Remove(wTUser);            

                wTUser.Active = false;
                await _context.SaveChangesAsync();

                return true;
            }

            public async Task<bool> UserExists(string username)
            {
                return await _context.Users.AnyAsync(e => e.Username == username);

            }
        #endregion

    }
}
