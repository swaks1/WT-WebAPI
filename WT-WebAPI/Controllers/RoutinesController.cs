using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WT_WebAPI.Common;
using WT_WebAPI.Entities.DTO.WorkoutAssets;
using WT_WebAPI.Entities.WorkoutAssets;
using WT_WebAPI.Repository.Interfaces;


namespace WT_WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Routines")]
    public class RoutinesController : Controller
    {
        private readonly ICommonRepository _repository;

        public RoutinesController(ICommonRepository repository)
        {
            _repository = repository;
        }


        [HttpGet("user/{userId}", Name = "GetRoutines")]
        public async Task<IActionResult> GetRoutines([FromRoute]int? userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var routines = await _repository.GetRoutinesFromUser(userId);

            if (routines == null)
            {
                return NotFound("Routines not found");
            }

            var mappedRoutines = Mapper.Map<IEnumerable<WorkoutRoutineDTO>>(routines);

            return Ok(mappedRoutines);
        }


        [HttpGet("user/{userId}/routine/{routineId}", Name = "GetRoutine")]
        public async Task<IActionResult> GetRoutine([FromRoute]int? userId, [FromRoute]int? routineId)
        {
            if (routineId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var routine = await _repository.GetRoutine(userId, routineId);

            if (routine == null)
            {
                return NotFound("Routine not found");
            }

            var mappedRoutine = Mapper.Map<WorkoutRoutineDTO>(routine);

            return Ok(mappedRoutine);
        }


        [HttpPost("user/{userId}", Name = "PostRoutine")]
        public async Task<IActionResult> PostRoutine([FromRoute]int? userId, [FromBody] WorkoutRoutineDTO routineDto)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (!await _repository.UserExists(userId))
            {
                return NotFound("User Doesn't Exist");
            }

            var routineEntity = Mapper.Map<WorkoutRoutine>(routineDto);
            var result = await _repository.AddRoutineForUser(userId, routineEntity);

            if (result == false)
            {
                return BadRequest("Add Failed for Routine...");
            }

            var routineToReturn = Mapper.Map<WorkoutRoutineDTO>(routineEntity);

            return CreatedAtRoute(
                            routeName: "GetRoutine",
                            routeValues: new
                            {
                                userId = userId,
                                routineId = routineToReturn.ID
                            },
                            value: routineToReturn);
        }


        [HttpPut("user/{userId}/routine/{routineId}", Name = "UpdateRoutine")]
        public async Task<IActionResult> UpdateRoutine([FromRoute] int? userId, [FromRoute] int? routineId, [FromBody] WorkoutRoutineDTO routineDto)
        {
            if (userId == null || routineId == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            routineDto.WTUserID = userId;
            routineDto.ID = (int)routineId;
            var wtURoutineEntity = Mapper.Map<WorkoutRoutine>(routineDto);
            var result = await _repository.UpdateRoutine(wtURoutineEntity);

            if (result == false)
            {
                return BadRequest("Update failed for routine...");
            }

            return NoContent();
        }


        [HttpPost("user/{userId}/routine/{routineId}/exercises", Name = "UpdateExercisesForRoutine")]
        public async Task<IActionResult> UpdateExercisesForRoutine([FromRoute]int? userId, [FromRoute] int? routineId, [FromBody] List<int> exerciseIds)
        {
            if (userId == null || routineId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var exerciseRoutineEntity = exerciseIds.Select(id => new ExerciseRoutineEntry { WorkoutRoutineID = routineId, ExerciseID = id });
            var result = await _repository.UpdateExercisesForRoutine(userId, routineId, exerciseRoutineEntity.ToList());

            if (result == false)
            {
                return BadRequest("Update of Exercises for Routine Failed...");
            }


            return NoContent();
        }


        [HttpPost("user/{userId}/routine/{routineId}/programs", Name = "UpdateProgramsForRoutine")]
        public async Task<IActionResult> UpdateProgramsForRoutine([FromRoute]int? userId, [FromRoute] int? routineId, [FromBody] List<int> programIds)
        {
            if (userId == null || routineId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var programRoutineEntity = programIds.Select(id => new RoutineProgramEntry { WorkoutRoutineID = routineId, WorkoutProgramID = id });
            var result = await _repository.UpdateProgramsForRoutine(userId, routineId, programRoutineEntity.ToList());

            if (result == false)
            {
                return BadRequest("Update of Programs for Routine Failed...");
            }


            return NoContent();
        }


        [HttpDelete("user/{userId}/routine/{routineId}")]
        public async Task<IActionResult> DeleteRoutine([FromRoute] int? userId, [FromRoute] int? routineId)
        {
            if (userId == null || routineId == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var result = await _repository.DeleteRoutine(userId, routineId);

            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}