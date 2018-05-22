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

            var routine = await _repository.GetRoutine(routineId);

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


        //[HttpPut("user/{userId}/exercise/{exerciseId}", Name = "UpdateExercise")]
        //public async Task<IActionResult> UpdateExercise([FromRoute] int? userId, [FromRoute] int? exerciseId, [FromBody] ExerciseDTO exerciseDTO)
        //{
        //    if (userId == null || exerciseId == null)
        //    {
        //        return BadRequest();
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return new UnprocessableEntityObjectResult(ModelState);
        //    }

        //    exerciseDTO.WTUserID = userId;
        //    exerciseDTO.ID = (int)exerciseId;
        //    var wtUserEntity = Mapper.Map<Exercise>(exerciseDTO);
        //    var result = await _repository.UpdateExercise(wtUserEntity);

        //    if (result == false)
        //    {
        //        return BadRequest("Update failed...");
        //    }

        //    return NoContent();
        //}


        //[HttpPost("user/{userId}/exercise/{exerciseId}/attributes", Name = "AddOrUpdateAttributes")]
        //public async Task<IActionResult> PostAttributes([FromRoute]int? userId, [FromRoute] int? exerciseId, [FromBody] List<ExerciseAttributeDTO> exerciseAttributesDTO)
        //{
        //    if (userId == null || exerciseId == null)
        //    {
        //        return BadRequest();
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return new UnprocessableEntityObjectResult(ModelState);
        //    }

        //    exerciseAttributesDTO.ForEach(item => item.ExerciseID = exerciseId);
        //    var exerciseAttributesEntities = Mapper.Map<List<ExerciseAttribute>>(exerciseAttributesDTO);
        //    var result = await _repository.AddOrUpdateAttributes(exerciseAttributesEntities);

        //    if (result == false)
        //    {
        //        return BadRequest("Add or Update of Attributes Failed...");
        //    }


        //    return NoContent();
        //}
    }
}