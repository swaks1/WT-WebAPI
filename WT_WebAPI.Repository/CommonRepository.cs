using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT_WebAPI.Entities;
using WT_WebAPI.Entities.DBContext;
using WT_WebAPI.Entities.WorkoutAssets;
using WT_WebAPI.Repository.Interfaces;
using WT_WebAPI.Entities.Extensions;

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
                                    .ThenInclude(e => e.ConcreteExercises)
                                        .ThenInclude(e => e.Attributes)
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

        public async Task<Exercise> GetExercise(int? userId, int? exerciseID)
        {
            return await _context.Exercises
                            .AsNoTracking()
                            .Include(e => e.Attributes)
                            .SingleOrDefaultAsync(e => e.ID == exerciseID && e.WTUserID == userId);

        }

        public async Task<bool> UpdateExercise(Exercise exercise)
        {
            var exerciseEntity = await _context.Exercises
                                                .Include(e => e.Attributes)
                                                .SingleOrDefaultAsync(e => e.ID == exercise.ID);

            if (exerciseEntity == null || exerciseEntity.WTUserID != exercise.WTUserID)
                return false;

            //Add or Update Attributes first
            exercise.Attributes.ToList().ForEach(item => item.ExerciseID = exercise.ID);

            var updateAttributesResult = await AddOrUpdateAttributes(exerciseEntity, exercise.Attributes.ToList());
            if (updateAttributesResult == false)
                return false;

            //Change other properties
            exerciseEntity.Category = exercise.Category;
            exerciseEntity.Description = exercise.Description;
            exerciseEntity.Name = exercise.Name;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddOrUpdateAttributes(int? userId, int? exerciseID, List<ExerciseAttribute> exerciseAttributes)
        {
            var exerciseEntity = await _context.Exercises
                                        .Include(e => e.Attributes)
                                        .SingleOrDefaultAsync(e => e.ID == exerciseID);

            if (exerciseEntity == null || exerciseEntity.WTUserID != userId)
            {
                return false;
            }

            return await AddOrUpdateAttributes(exerciseEntity, exerciseAttributes);
        }

        public async Task<bool> AddOrUpdateAttributes(Exercise exerciseEntity, List<ExerciseAttribute> exerciseAttributes)
        {
            if (exerciseEntity == null)
                return false;

            //remove attributes that arent present
            var attrToBeRemoved = exerciseEntity.Attributes.Where(attr => !exerciseAttributes.Any(a => a.ID == attr.ID)).ToList();
            _context.RemoveRange(attrToBeRemoved);

            //update existing and make list for new attributes
            List<ExerciseAttribute> toBeAdded = new List<ExerciseAttribute>();
            foreach (var attribute in exerciseAttributes)
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
            foreach (var attribute in toBeAdded)
            {
                exerciseEntity.Attributes.Add(attribute);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteExercise(int? userId, int? exerciseID)
        {
            var exerciseEntity = await _context.Exercises
                                               .SingleOrDefaultAsync(m => m.WTUserID == userId && m.ID == exerciseID);

            if (exerciseEntity == null || exerciseEntity.WTUserID != userId)
            {
                return false;
            }

            _context.ExerciseAttribute.RemoveRange(_context.ExerciseAttribute.Where(item => item.ExerciseID == exerciseID));
            _context.ExerciseRoutineEntry.RemoveRange(_context.ExerciseRoutineEntry.Where(item => item.ExerciseID == exerciseID));
            _context.Remove(exerciseEntity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAttribite(int? userId, int? exerciseID, int? attributeId)
        {
            var exerciseEntity = await _context.Exercises
                                               .Include(ex => ex.Attributes)
                                               .SingleOrDefaultAsync(m => m.ID == exerciseID);

            if (exerciseEntity == null || exerciseEntity.WTUserID != userId)
            {
                return false;
            }

            var attrEntity = exerciseEntity.Attributes.FirstOrDefault(attr => attr.ID == attributeId);
            if (attrEntity != null)
            {
                _context.Remove(attrEntity);
            }

            await _context.SaveChangesAsync();

            return true;
        }
        #endregion


        #region Routines

        public async Task<IEnumerable<WorkoutRoutine>> GetRoutinesFromUser(int? userId)
        {
            return await _context.WorkoutRoutines
                            .AsNoTracking()
                            .Include(rout => rout.ExerciseRoutineEntries)
                                .ThenInclude(rout => rout.Exercise)
                                    .ThenInclude(ex => ex.Attributes)
                            .Include(rout => rout.RoutineProgramEntries)
                            .Where(rout => rout.WTUserID == userId)
                            .ToListAsync();
        }

        public async Task<WorkoutRoutine> GetRoutine(int? userId, int? routineId)
        {
            return await _context.WorkoutRoutines
                                    .AsNoTracking()
                                    .Include(rout => rout.ExerciseRoutineEntries)
                                        .ThenInclude(rout => rout.Exercise)
                                                .ThenInclude(ex => ex.Attributes)
                                     .Include(rout => rout.RoutineProgramEntries)
                                    .SingleOrDefaultAsync(e => e.ID == routineId && e.WTUserID == userId);
        }

        public async Task<bool> AddRoutineForUser(int? userId, WorkoutRoutine routine)
        {
            var user = await GetUserByUserId(userId);

            if (user == null)
                return false;

            routine.ID = 0;
            routine.WTUserID = userId;
            routine.ExerciseRoutineEntries.ToList().ForEach(item => item.WorkoutRoutineID = routine.ID);
            routine.RoutineProgramEntries.ToList().ForEach(item => item.WorkoutRoutineID = routine.ID);


            //check if the posted programIDs or exerciseIDs belong to the user
            var allUserProgramsIds = await _context.WorkoutPrograms
                                        .AsNoTracking()
                                        .Where(item => item.WTUserID == userId)
                                        .Select(item => item.ID)
                                        .ToListAsync();

            var allUserExercisesIDs = await _context.Exercises
                                        .AsNoTracking()
                                        .Where(item => item.WTUserID == userId)
                                        .Select(item => item.ID)
                                        .ToListAsync();

            var programsCheck = routine.RoutineProgramEntries.All(item => allUserProgramsIds.Contains((int)item.WorkoutProgramID));
            var exerciseCheck = routine.ExerciseRoutineEntries.All(item => allUserExercisesIDs.Contains((int)item.ExerciseID));

            if (!programsCheck || !exerciseCheck)
            {
                return false;
            }


            _context.WorkoutRoutines.Add(routine);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateRoutine(WorkoutRoutine routine)
        {
            var routineEntity = await _context.WorkoutRoutines
                                    .Include(w => w.ExerciseRoutineEntries)
                                    .Include(w => w.RoutineProgramEntries)
                                    .SingleOrDefaultAsync(w => w.ID == routine.ID);

            if (routineEntity == null || routineEntity.WTUserID != routine.WTUserID)
                return false;

            routine.ExerciseRoutineEntries.ToList().ForEach(item => item.WorkoutRoutineID = routineEntity.ID);
            var updateExercisesResult = await UpdateExercisesForRoutine(routineEntity, routine.ExerciseRoutineEntries.ToList());
            if (updateExercisesResult == false)
                return false;

            routine.RoutineProgramEntries.ToList().ForEach(item => item.WorkoutRoutineID = routineEntity.ID);
            var updateProgramsResult = await UpdateProgramsForRoutine(routineEntity, routine.RoutineProgramEntries.ToList());
            if (updateProgramsResult == false)
                return false;

            routineEntity.Name = routine.Name;
            routineEntity.PlannedDates = routine.PlannedDates;
            routineEntity.Description = routine.Description;

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> UpdateExercisesForRoutine(int? userId, int? routineId, List<ExerciseRoutineEntry> routineExercises)
        {
            var routineEntity = await _context.WorkoutRoutines
                        .Include(w => w.ExerciseRoutineEntries)
                        .SingleOrDefaultAsync(w => w.ID == routineId);

            if (routineEntity == null || routineEntity.WTUserID != userId)
            {
                return false;
            }

            return await UpdateExercisesForRoutine(routineEntity, routineExercises);
        }

        public async Task<bool> UpdateExercisesForRoutine(WorkoutRoutine routineEntity, List<ExerciseRoutineEntry> routineExercises)
        {
            if (routineEntity == null)
                return false;

            //remove exercises that arent present
            var exercisesToBeRemoved = routineEntity.ExerciseRoutineEntries.Where(item => !routineExercises.Any(re => re.ExerciseID == item.ExerciseID)).ToList();
            _context.RemoveRange(exercisesToBeRemoved);

            //add new exercises
            var toBeAdded = routineExercises.Where(item => !routineEntity.ExerciseRoutineEntries.Any(ex => ex.ExerciseID == item.ExerciseID)).ToList();


            var allUserExercisesIDs = await _context.Exercises
                                        .AsNoTracking()
                                        .Where(item => item.WTUserID == routineEntity.WTUserID)
                                        .Select(item => item.ID)
                                        .ToListAsync();

            //check if all exercises to be added belong to this user
            var exercisesCheck = toBeAdded.All(item => allUserExercisesIDs.Contains((int)item.ExerciseID));
            if (exercisesCheck == false)
                return false;

            toBeAdded.ForEach(item => routineEntity.ExerciseRoutineEntries.Add(item));

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> UpdateProgramsForRoutine(int? userId, int? routineId, List<RoutineProgramEntry> routinePrograms)
        {

            var routineEntity = await _context.WorkoutRoutines
                                    .Include(w => w.RoutineProgramEntries)
                                    .SingleOrDefaultAsync(w => w.ID == routineId);

            if (routineEntity == null || routineEntity.WTUserID != userId)
            {
                return false;
            }

            return await UpdateProgramsForRoutine(routineEntity, routinePrograms);

        }

        public async Task<bool> UpdateProgramsForRoutine(WorkoutRoutine routineEntity, List<RoutineProgramEntry> routinePrograms)
        {
            if (routineEntity == null)
                return false;

            //remove programs that arent present
            var programsToBeRemoved = routineEntity.RoutineProgramEntries.Where(item => !routinePrograms.Any(rp => rp.WorkoutProgramID == item.WorkoutProgramID)).ToList();
            _context.RemoveRange(programsToBeRemoved);

            //add new programs
            var toBeAdded = routinePrograms.Where(item => !routineEntity.RoutineProgramEntries.Any(rp => rp.WorkoutProgramID == item.WorkoutProgramID)).ToList();


            var allUserProgramsIds = await _context.WorkoutPrograms
                                       .AsNoTracking()
                                       .Where(item => item.WTUserID == routineEntity.WTUserID)
                                       .Select(item => item.ID)
                                       .ToListAsync();

            //check if all programs to be added belong to this user
            var programsCheck = toBeAdded.All(item => allUserProgramsIds.Contains((int)item.WorkoutProgramID));
            if (programsCheck == false)
                return false;

            toBeAdded.ForEach(item => routineEntity.RoutineProgramEntries.Add(item));

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> DeleteRoutine(int? userId, int? routineID)
        {
            var routineEntity = await _context.WorkoutRoutines
                                   .SingleOrDefaultAsync(m => m.ID == routineID);

            if (routineEntity == null || routineEntity.WTUserID != userId)
            {
                return false;
            }

            _context.ExerciseRoutineEntry.RemoveRange(_context.ExerciseRoutineEntry.Where(item => item.WorkoutRoutineID == routineID));
            _context.RoutineProgramEntry.RemoveRange(_context.RoutineProgramEntry.Where(item => item.WorkoutRoutineID == routineID));
            _context.Remove(routineEntity);

            await _context.SaveChangesAsync();

            return true;
        }

        #endregion



        #region Programs

        public async Task<IEnumerable<WorkoutProgram>> GetProgramsFromUser(int? userId)
        {
            return await _context.WorkoutPrograms
                                .AsNoTracking()
                                .Include(w => w.RoutineProgramEntries)
                                    .ThenInclude(rout => rout.WorkoutRoutine)
                                .Where(rout => rout.WTUserID == userId)
                                .ToListAsync();
        }

        public async Task<WorkoutProgram> GetProgram(int? userId, int? programId)
        {
            return await _context.WorkoutPrograms
                                .AsNoTracking()
                                .Include(w => w.RoutineProgramEntries)
                                    .ThenInclude(rout => rout.WorkoutRoutine)
                                .SingleOrDefaultAsync(e => e.ID == programId && e.WTUserID == userId);
        }

        public async Task<bool> AddProgramForUser(int? userId, WorkoutProgram program)
        {
            var user = await GetUserByUserId(userId);

            if (user == null)
                return false;

            program.ID = 0;
            program.WTUserID = userId;
            program.RoutineProgramEntries.ToList().ForEach(item => item.WorkoutProgramID = program.ID);


            //check if the posted routinesIds belong to the user
            var allUserRoutineIds = await _context.WorkoutRoutines
                                        .AsNoTracking()
                                        .Where(item => item.WTUserID == userId)
                                        .Select(item => item.ID)
                                        .ToListAsync();


            var routinesCheck = program.RoutineProgramEntries.All(item => allUserRoutineIds.Contains((int)item.WorkoutRoutineID));

            if (!routinesCheck)
            {
                return false;
            }

            _context.WorkoutPrograms.Add(program);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateProgram(WorkoutProgram program)
        {
            var programEntity = await _context.WorkoutPrograms
                                               .Include(w => w.RoutineProgramEntries)
                                               .SingleOrDefaultAsync(w => w.ID == program.ID);

            if (programEntity == null || programEntity.WTUserID != program.WTUserID)
                return false;

            program.RoutineProgramEntries.ToList().ForEach(item => item.WorkoutProgramID = programEntity.ID);
            var updateRoutinesResult = await UpdateRoutinesForProgram(programEntity, program.RoutineProgramEntries.ToList());
            if (updateRoutinesResult == false)
                return false;

            programEntity.Name = program.Name;
            programEntity.Description = program.Description;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateRoutinesForProgram(int? userId, int? programId, List<RoutineProgramEntry> routineProgramsEntries)
        {
            var programEntity = await _context.WorkoutPrograms
                                        .Include(w => w.RoutineProgramEntries)
                                        .SingleOrDefaultAsync(w => w.ID == programId);

            if (programEntity == null || programEntity.WTUserID != userId)
            {
                return false;
            }

            return await UpdateRoutinesForProgram(programEntity, routineProgramsEntries);
        }

        public async Task<bool> UpdateRoutinesForProgram(WorkoutProgram programEntity, List<RoutineProgramEntry> routineProgramEntries)
        {
            if (programEntity == null)
                return false;

            //remove routines that arent present
            var routinesToBeRemoved = programEntity.RoutineProgramEntries.Where(item => !routineProgramEntries.Any(rp => rp.WorkoutRoutineID == item.WorkoutRoutineID)).ToList();
            _context.RemoveRange(routinesToBeRemoved);

            //add new or update existing routines
            List<RoutineProgramEntry> toBeAdded = new List<RoutineProgramEntry>();
            foreach (var rp in routineProgramEntries)
            {
                var rpEntity = programEntity.RoutineProgramEntries.FirstOrDefault(a => a.WorkoutRoutineID == rp.WorkoutRoutineID);
                if (rpEntity != null)
                {
                    rpEntity.PlannedDates = rp.PlannedDates;
                }
                else
                {
                    rp.ID = 0;
                    toBeAdded.Add(rp);
                }
            }

            var allUserRoutineIDs = await _context.WorkoutRoutines
                                               .AsNoTracking()
                                               .Where(item => item.WTUserID == programEntity.WTUserID)
                                               .Select(item => item.ID)
                                               .ToListAsync();

            //check if all routines to be added belong to this user
            var routinesCheck = toBeAdded.All(item => allUserRoutineIDs.Contains((int)item.WorkoutRoutineID));
            if (routinesCheck == false)
                return false;

            toBeAdded.ForEach(item => programEntity.RoutineProgramEntries.Add(item));

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProgram(int? userId, int? programId)
        {
            var programEntity = await _context.WorkoutPrograms
                                              .SingleOrDefaultAsync(m => m.ID == programId);

            if (programEntity == null || programEntity.WTUserID != userId)
            {
                return false;
            }

            _context.RoutineProgramEntry.RemoveRange(_context.RoutineProgramEntry.Where(item => item.WorkoutProgramID == programId));
            _context.Remove(programEntity);

            await _context.SaveChangesAsync();

            return true;
        }


        #endregion

        #region WorkoutSessions

        public async Task<IEnumerable<WorkoutSession>> GetSessionsForUser(int? userId, DateTime? startDate, DateTime? endDate)
        {

            var allSessions = _context.WorkoutSessions
                                        .AsNoTracking()
                                        .Include(w => w.ConcreteExercises)
                                            .ThenInclude(c => c.Attributes)
                                        .Where(w => w.WTUserID == userId && w.Date != null);
            if (startDate != null)
            {
                allSessions = allSessions.Where(session => session.Date.Value.Date >= startDate.Value.Date);
            }

            if (endDate != null)
            {
                allSessions = allSessions.Where(session => session.Date.Value.Date <= endDate.Value.Date);
            }

            return await allSessions.ToListAsync();
        }

        public async Task<WorkoutSession> GetSession(int? userId, int? sessionId)
        {
            return await _context.WorkoutSessions
                                .Include(w => w.ConcreteExercises)
                                        .ThenInclude(c => c.Attributes)
                                .SingleOrDefaultAsync(w => w.ID == sessionId && w.WTUserID == userId);
        }

        public async Task<WorkoutSession> GetSessionForDay(int? userId, DateTime date)
        {
            var session = await _context.WorkoutSessions
                                        .Include(w => w.ConcreteExercises)
                                                .ThenInclude(c => c.Attributes)
                                        .SingleOrDefaultAsync(w => w.WTUserID == userId && w.Date.Value.Date == date.Date);

            if (session == null)
            {
                session = new WorkoutSession
                {
                    Date = date,
                    WTUserID = userId
                };
                _context.WorkoutSessions.Add(session);
                await _context.SaveChangesAsync();
            }

            return session;
        }

        public async Task<WorkoutSession> AddOrUpdateSession(int? userId, DateTime date, List<WorkoutRoutine> routines, List<Exercise> exercises, List<ConcreteExercise> concreteExercises)
        {
            var session = await GetSessionForDay(userId, date);

            if (routines == null)
                routines = new List<WorkoutRoutine>();
            if (exercises == null)
                exercises = new List<Exercise>();
            if (concreteExercises == null)
                concreteExercises = new List<ConcreteExercise>();

            //get Routines form db and convert them to ConcreteExercises 
            var routinesIds = routines.Select(item => item.ID);
            var routinesEntityList = await _context.WorkoutRoutines
                                                    .AsNoTracking()
                                                    .Include(rout => rout.ExerciseRoutineEntries)
                                                        .ThenInclude(rout => rout.Exercise)
                                                            .ThenInclude(ex => ex.Attributes)
                                                    .Where(rout => rout.WTUserID == userId && routinesIds.Contains(rout.ID))
                                                    .ToListAsync();

            var concreteExercisesFromRoutine = new List<ConcreteExercise>();
            foreach (var routine in routinesEntityList)
            {
                var rotuineEntries = routine.ExerciseRoutineEntries.ToList();
                foreach (var er in rotuineEntries)
                {
                    var concreteExercise = er.Exercise.GetConcreteExerciseObject();
                    concreteExercise.RoutineId = routine.ID;
                    concreteExercise.RoutineName = routine.Name;
                    concreteExercise.WorkoutSessionID = session.ID;
                    concreteExercisesFromRoutine.Add(concreteExercise);
                }
            }


            //convert Exercises to ConcreteExercises 
            var exerciseIds = exercises.Select(item => item.ID);
            var exerciseEntityList = await _context.Exercises
                                                    .AsNoTracking()
                                                    .Include(rout => rout.Attributes)
                                                    .Where(rout => rout.WTUserID == userId && exerciseIds.Contains(rout.ID))
                                                    .ToListAsync();

            var concreteExercisesFromExercises = new List<ConcreteExercise>();
            foreach (var exercise in exerciseEntityList)
            {
                var concreteExercise = exercise.GetConcreteExerciseObject();
                concreteExercise.WorkoutSessionID = session.ID;
                concreteExercisesFromExercises.Add(concreteExercise);
            }



            //Update existing concreteExercises found in the session
            foreach (var cExercise in concreteExercises)
            {
                var concreteExerciseEntity = session.ConcreteExercises.FirstOrDefault(item => item.ID == cExercise.ID);
                if (concreteExerciseEntity != null)
                {
                    cExercise.Attributes.ToList().ForEach(item => item.ID = 0);

                    concreteExerciseEntity.Name = cExercise.Name;
                    concreteExerciseEntity.Category = cExercise.Category;
                    concreteExerciseEntity.Description = cExercise.Description;

                    _context.ConcreteExerciseAttribute.RemoveRange(concreteExerciseEntity.Attributes);
                    concreteExerciseEntity.Attributes = cExercise.Attributes;
                }
            }


            //add the new Concrete Exercises which were created before...
            concreteExercisesFromRoutine.ToList().ForEach(item => session.ConcreteExercises.Add(item));
            concreteExercisesFromExercises.ToList().ForEach(item => session.ConcreteExercises.Add(item));


            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<bool> UpdateConcreteExercises(int? userId, int? sessionId, List<ConcreteExercise> concreteExercises)
        {
            var sessionEntity = await GetSession(userId, sessionId);

            if (sessionEntity == null)
                return false;

            foreach (var cExercise in concreteExercises)
            {
                var concreteExerciseEntity = sessionEntity.ConcreteExercises.FirstOrDefault(item => item.ID == cExercise.ID);
                if (concreteExerciseEntity != null)
                {
                    cExercise.Attributes.ToList().ForEach(item => item.ID = 0);

                    concreteExerciseEntity.Name = cExercise.Name;
                    concreteExerciseEntity.Category = cExercise.Category;
                    concreteExerciseEntity.Description = cExercise.Description;

                    _context.ConcreteExerciseAttribute.RemoveRange(concreteExerciseEntity.Attributes);
                    concreteExerciseEntity.Attributes = cExercise.Attributes;
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteConcreteExercises(int? userId, int? sessionId, List<int> concreteExerciseIds)
        {
            var sessionEntity = await GetSession(userId, sessionId);

            if (sessionEntity == null)
                return false;

            foreach (var id in concreteExerciseIds)
            {
                var concreteExerciseEntity = sessionEntity.ConcreteExercises.FirstOrDefault(item => item.ID == id);
                if (concreteExerciseEntity != null)
                {
                    _context.ConcreteExerciseAttribute.RemoveRange(concreteExerciseEntity.Attributes);
                    _context.Remove(concreteExerciseEntity);
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }

        #endregion
    }
}
