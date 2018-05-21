using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT_WebAPI.Entities;
using WT_WebAPI.Entities.DBContext;
using WT_WebAPI.Entities.WorkoutAssets;
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
                            .AsNoTracking()
                            .SingleOrDefaultAsync(user => user.Username == username);
        }

        public async Task<WTUser> GetUserByUserId(int? userId)
        {
            return await _context.Users
                            .AsNoTracking()
                            .SingleOrDefaultAsync(user => user.ID == userId);
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

        public async Task<bool> UpdateUser(WTUser user)
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

        public async Task<bool> UserExists(int? userId)
        {
            return await _context.Users.AnyAsync(e => e.ID == userId);
        }

        #endregion


        #region Exercises

        public async Task<IEnumerable<Exercise>> GetExercisesFromUser(int? userId)
        {
            return await _context.Exercises
                                .AsNoTracking()
                                .Include(e => e.Attributes)
                                .Where(e => e.WTUserID == userId)
                                .ToListAsync();
        }

        public async Task<bool> AddExerciseForUser(int? userId, Exercise exercise)
        {
            var user = await GetUserByUserId(userId);

            if (user == null)
                return false;

            exercise.ID = 0;
            exercise.WTUserID = userId;
            foreach (var attr in exercise.Attributes)
            {
                attr.ID = 0;
            }
            _context.Exercises.Add(exercise);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Exercise> GetExercise(int? exerciseID)
        {
            return await _context.Exercises
                            .AsNoTracking()
                            .Include(e=>e.Attributes)
                            .SingleOrDefaultAsync(e => e.ID == exerciseID);
        }

        public async Task<bool> UpdateExercise(Exercise exercise)
        {
            var exerciseEntity = await _context.Exercises
                                                .Include(e => e.Attributes)
                                                .SingleOrDefaultAsync(e => e.ID == exercise.ID);

            if (exerciseEntity == null || exerciseEntity.WTUserID != exercise.WTUserID)
                return false;

            //remove attributes that arent present
            var attrToBeRemoved = exerciseEntity.Attributes.Where(attr => !exercise.Attributes.Any(a => a.ID == attr.ID)).ToList();
            _context.RemoveRange(attrToBeRemoved);

            //update existing and make list for new attributes
            List<ExerciseAttribute> toBeAdded = new List<ExerciseAttribute>();
            foreach(var attribute in exercise.Attributes) 
            {
                var attrEntity = exerciseEntity.Attributes.FirstOrDefault(a => a.ID == attribute.ID);
                if (attrEntity != null)
                {
                    attrEntity.AttributeValue = attribute.AttributeValue;
                    attrEntity.AttributeName = attribute.AttributeName;                   
                }
                else
                {
                    attribute.ID = 0;
                    toBeAdded.Add(attribute);
                }
            }

            //add new attributes
            foreach(var attribute in toBeAdded)
            {
                exerciseEntity.Attributes.Add(attribute);
            }


            exerciseEntity.Category = exercise.Category;
            exerciseEntity.Description = exercise.Description;
            exerciseEntity.Name = exercise.Name;

            await _context.SaveChangesAsync();

            return true;
        }

        #endregion
    }
}
