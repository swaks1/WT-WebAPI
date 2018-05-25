using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WT_WebAPI.Entities;
using WT_WebAPI.Entities.WorkoutAssets;

namespace WT_WebAPI.Repository.Interfaces
{
    public interface ICommonRepository
    {
        #region Users

        Task<WTUser> GetUserByUsername(string username);

        Task<WTUser> GetUserByUsernameFullInfo(string username);

        Task<bool> UpdateUser(WTUser user);

        Task<bool> DeleteUser(string username);

        Task<bool> UserExists(string username);

        Task<bool> UserExists(int? userId);

        #endregion



        #region Exercises

        Task<IEnumerable<Exercise>> GetExercisesFromUser(int? userId);

        Task<Exercise> GetExercise(int? userId, int? exerciseId);

        Task<bool> AddExerciseForUser(int? userId, Exercise exercise);

        Task<bool> UpdateExercise(Exercise exercise);

        Task<bool> AddOrUpdateAttributes(int? userId, int? exerciseID, List<ExerciseAttribute> exerciseAttributes);

        Task<bool> AddOrUpdateAttributes(Exercise exerciseEntity, List<ExerciseAttribute> exerciseAttributes);

        Task<bool> DeleteExercise(int? userId, int? exerciseID);

        Task<bool> DeleteAttribite(int? userId, int? exerciseID, int? attributeId);

        #endregion


        #region Routines

        Task<IEnumerable<WorkoutRoutine>> GetRoutinesFromUser(int? userId);

        Task<WorkoutRoutine> GetRoutine(int? userId, int? routineId);

        Task<bool> AddRoutineForUser(int? userId, WorkoutRoutine routine);

        Task<bool> UpdateRoutine(WorkoutRoutine routine);

        Task<bool> UpdateExercisesForRoutine(int? userId, int? routineId, List<ExerciseRoutineEntry> routineExercises);

        Task<bool> UpdateExercisesForRoutine(WorkoutRoutine routineEntity, List<ExerciseRoutineEntry> routineExercises);

        Task<bool> UpdateProgramsForRoutine(int? userId, int? routineId, List<RoutineProgramEntry> routinePrograms);

        Task<bool> UpdateProgramsForRoutine(WorkoutRoutine routineEntity, List<RoutineProgramEntry> routinePrograms);

        Task<bool> DeleteRoutine(int? userId, int? routineID);

        #endregion


        #region Programs

        Task<IEnumerable<WorkoutProgram>> GetProgramsFromUser(int? userId);

        Task<WorkoutProgram> GetProgram(int? userId, int? programId);

        Task<bool> AddProgramForUser(int? userId, WorkoutProgram program);

        Task<bool> UpdateProgram(WorkoutProgram program);

        Task<bool> UpdateRoutinesForProgram(int? userId, int? programId, List<RoutineProgramEntry> routinePrograms);

        Task<bool> UpdateRoutinesForProgram(WorkoutProgram programEntity, List<RoutineProgramEntry> routinePrograms);

        Task<bool> DeleteProgram(int? userId, int? programId);

        #endregion


        #region WorkoutSessions

        Task<IEnumerable<WorkoutSession>> GetSessionsForUser(int? userId, DateTime? startDate, DateTime? endDate);

        Task<WorkoutSession> GetSession(int? userId, int? sessionId);

        Task<WorkoutSession> GetSessionForDay(int? userId, DateTime date);

        Task<WorkoutSession> AddOrUpdateSession(int? userId, 
                                                DateTime date, 
                                                List<WorkoutRoutine> routines, 
                                                List<Exercise> exercises, 
                                                List<ConcreteExercise> concreteExercises);

        Task<bool> UpdateConcreteExercises(int? userId, int? sessionId, List<ConcreteExercise> concreteExercises);

        Task<bool> DeleteConcreteExercises(int? userId, int? sessionId, List<int> concreteExerciseIds);

        #endregion
    }
}
