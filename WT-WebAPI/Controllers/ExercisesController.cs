using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WT_WebAPI.Common;
using WT_WebAPI.Entities.DTO.WorkoutAssets;
using WT_WebAPI.Entities.WorkoutAssets;
using WT_WebAPI.Repository.Interfaces;

namespace WT_WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Exercises")]
    public class ExercisesController : Controller
    {
        private readonly ICommonRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<ExercisesController> _logger;


        public ExercisesController(ICommonRepository repository, IHostingEnvironment hostingEnvironment, ILogger<ExercisesController> logger)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
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

            var exercise = await _repository.GetExercise(userId, exerciseId);

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

            exerciseDTO.WTUserID = userId;
            var exerciseEntity = Mapper.Map<Exercise>(exerciseDTO);

            var result = await _repository.AddExerciseForUser(userId, exerciseEntity);

            if (result == false)
            {
                return BadRequest("Add Failed...");
            }


            if (exerciseEntity.ImageBytes != null && exerciseEntity.ImageBytes.Length != 0)
            {
                var imageResult = await SaveExerciseImage(exerciseEntity);
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
            var wtExerciseEntity = Mapper.Map<Exercise>(exerciseDTO);
            var result = await _repository.UpdateExercise(wtExerciseEntity);

            if (result == false)
            {
                return BadRequest("Update failed...");
            }

            if (wtExerciseEntity.ImageBytes != null && wtExerciseEntity.ImageBytes.Length != 0)
            {
                var imageResult = await SaveExerciseImage(wtExerciseEntity);
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
     
            RemoveExerciseImage(userId, exerciseId);
            
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


        private async Task<bool> SaveExerciseImage(Exercise exerciseEntity)
        {
            try
            {
                // get this environment's web root path (the path
                // from which static content, like an image, is served)
                var webRootPath = _hostingEnvironment.WebRootPath;
                string folderName = "Images/Exercises/" + exerciseEntity.WTUserID.ToString();
                string fullFolderPath = $"{webRootPath}/{folderName}/";

                // create path if not exists... write bytes and auto-close stream
                FileInfo file = new FileInfo(fullFolderPath);
                file.Directory.Create();

                // create the filename
                string imageName = exerciseEntity.ImagePath;
                var imageExtension = imageName.Substring(imageName.LastIndexOf("."));

                string fileName = "exercise_" + exerciseEntity.ID + "_" + Guid.NewGuid() + imageExtension;

                //delete previuos image
                string[] fileList = Directory.GetFiles(fullFolderPath, $"*exercise_{exerciseEntity.ID}*");
                foreach (var fileToDelete in fileList)
                {
                    System.IO.File.Delete(fileToDelete);
                }

                // the full file path
                var filePath = Path.Combine($"{fullFolderPath}/{fileName}");

                System.IO.File.WriteAllBytes(filePath, exerciseEntity.ImageBytes);

                // fill out the filename
                exerciseEntity.ImagePath = folderName + "/" + fileName;

                await _repository.UpdateImageForExercise(exerciseEntity.ID, exerciseEntity.ImagePath);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, ex.Message);
                return false;
            }

        }

        private bool RemoveExerciseImage(int? userId, int? exerciseId)
        {
            try
            {
                // get this environment's web root path (the path
                // from which static content, like an image, is served)
                var webRootPath = _hostingEnvironment.WebRootPath;
                string folderName = "Images/Exercises/" + userId;
                string fullFolderPath = $"{webRootPath}/{folderName}/";

                // create path if not exists... write bytes and auto-close stream
                FileInfo file = new FileInfo(fullFolderPath);
                file.Directory.Create();

                //delete previuos image
                string[] fileList = Directory.GetFiles(fullFolderPath, $"*exercise_{exerciseId}*");
                foreach (var fileToDelete in fileList)
                {
                    System.IO.File.Delete(fileToDelete);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, ex.Message);
                return false;
            }

        }
    }
}
