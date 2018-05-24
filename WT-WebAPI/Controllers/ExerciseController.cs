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
    [Route("api/Exercises")]
    public class ExerciseController : Controller
    {
        private readonly ICommonRepository _repository;

        public ExerciseController(ICommonRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("user/{userId}", Name = "GetExercises")]
        public async Task<IActionResult> GetExercises([FromRoute]int? userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var exercises = await _repository.GetExercisesFromUser(userId);

            if (exercises == null)
            {
                return NotFound("Exercises not found");
            }

            var mappedExercises = Mapper.Map<IEnumerable<ExerciseDTO>>(exercises);

            return Ok(mappedExercises);
        }


        [HttpGet("user/{userId}/exercise/{exerciseId}", Name = "GetExercise")]
        public async Task<IActionResult> GetExercise([FromRoute]int? userId, [FromRoute]int? exerciseId)
        {
            if (exerciseId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var exercise = await _repository.GetExercise(exerciseId);

            if (exercise == null)
            {
                return NotFound("Exercise not found");
            }

            var mappedExercise = Mapper.Map<ExerciseDTO>(exercise);

            return Ok(mappedExercise);
        }



        [HttpPost("user/{userId}", Name = "PostExercise")]
        public async Task<IActionResult> PostExercise([FromRoute]int? userId, [FromBody] ExerciseDTO exerciseDTO)
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

            var exerciseEntity = Mapper.Map<Exercise>(exerciseDTO);
            var result = await _repository.AddExerciseForUser(userId, exerciseEntity);

            if (result == false)
            {
                return BadRequest("Add Failed...");
            }

            var exerciseToReturn = Mapper.Map<ExerciseDTO>(exerciseEntity);

            return CreatedAtRoute(
                            routeName: "GetExercise",
                            routeValues: new
                            {
                                userId = userId,
                                exerciseId = exerciseToReturn.ID
                            },
                            value: exerciseToReturn);
        }


        [HttpPut("user/{userId}/exercise/{exerciseId}", Name = "UpdateExercise")]
        public async Task<IActionResult> UpdateExercise([FromRoute] int? userId, [FromRoute] int? exerciseId, [FromBody] ExerciseDTO exerciseDTO)
        {
            if (userId == null || exerciseId == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            exerciseDTO.WTUserID = userId;
            exerciseDTO.ID = (int)exerciseId;
            var wtUserEntity = Mapper.Map<Exercise>(exerciseDTO);
            var result = await _repository.UpdateExercise(wtUserEntity);

            if (result == false)
            {
                return BadRequest("Update failed...");
            }

            return NoContent();
        }


        [HttpPost("user/{userId}/exercise/{exerciseId}/attributes", Name = "AddOrUpdateAttributes")]
        public async Task<IActionResult> PostAttributes([FromRoute]int? userId, [FromRoute] int? exerciseId, [FromBody] List<ExerciseAttributeDTO> exerciseAttributesDTO)
        {
            if (userId == null || exerciseId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            exerciseAttributesDTO.ForEach(item => item.ExerciseID = exerciseId);
            var exerciseAttributesEntities = Mapper.Map<List<ExerciseAttribute>>(exerciseAttributesDTO);

            var result = await _repository.AddOrUpdateAttributes(userId, exerciseId, exerciseAttributesEntities);

            if (result == false)
            {
                return BadRequest("Add or Update of Attributes Failed...");
            }


            return NoContent();
        }

        [HttpDelete("user/{userId}/exercise/{exerciseId}")]
        public async Task<IActionResult> DeleteExercise([FromRoute] int? userId, [FromRoute] int? exerciseId)
        {
            if (userId == null || exerciseId == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var result = await _repository.DeleteExercise(userId, exerciseId);

            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("user/{userId}/exercise/{exerciseId}/attribute/{attributeId}")]
        public async Task<IActionResult> DeleteAttribute([FromRoute] int? userId, [FromRoute] int? exerciseId, [FromRoute] int? attributeId)
        {
            if (userId == null || exerciseId == null || attributeId == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var result = await _repository.DeleteAttribite(userId, exerciseId, attributeId);

            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
