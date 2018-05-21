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
    }
}
