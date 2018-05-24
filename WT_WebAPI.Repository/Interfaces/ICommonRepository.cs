using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WT_WebAPI.Entities;
using WT_WebAPI.Entities.DTO.WorkoutAssets;
using WT_WebAPI.Entities.WorkoutAssets;

namespace WT_WebAPI.Repository.Interfaces
{
    public interface ICommonRepository
    {
        Task<WTUser> GetUserByUsername(string username);

        Task<WTUser> GetUserByUsernameFullInfo(string username);

        Task<bool> UpdateUser(WTUser user);

        Task<bool> DeleteUser(string username);

        Task<bool> UserExists(string username);

        Task<bool> UserExists(int? userId);



        Task<IEnumerable<Exercise>> GetExercisesFromUser(int? userId);

        Task<Exercise> GetExercise(int? userId);

        Task<bool> AddExerciseForUser(int? userId, Exercise exercise);

        Task<bool> UpdateExercise(Exercise exercise);

        Task<bool> AddOrUpdateAttributes(int? userId, int? exerciseID, List<ExerciseAttribute> exerciseAttributes);

        Task<bool> AddOrUpdateAttributes(Exercise exerciseEntity, List<ExerciseAttribute> exerciseAttributes);

        Task<bool> DeleteExercise(int? userId, int? exerciseID);

        Task<bool> DeleteAttribite(int? userId, int? exerciseID, int? attributeId);


        Task<IEnumerable<WorkoutRoutine>> GetRoutinesFromUser(int? userId);

        Task<WorkoutRoutine> GetRoutine(int? routineId);

        Task<bool> AddRoutineForUser(int? userId, WorkoutRoutine routine);

        Task<bool> UpdateRoutine(WorkoutRoutine routine);

        Task<bool> UpdateExercisesForRoutine(int? userId, int? routineId, List<ExerciseRoutineEntry> routineExercises);

        Task<bool> UpdateExercisesForRoutine(WorkoutRoutine routineEntity, List<ExerciseRoutineEntry> routineExercises);

        Task<bool> UpdateProgramsForRoutine(int? userId, int? routineId, List<RoutineProgramEntry> routinePrograms);

        Task<bool> UpdateProgramsForRoutine(WorkoutRoutine routineEntity, List<RoutineProgramEntry> routinePrograms);

        Task<bool> DeleteRoutine(int? userId, int? routineID);
    }
}
