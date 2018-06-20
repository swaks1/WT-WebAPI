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
    [Route("api/Programs")]
    public class ProgramsController : Controller
    {
        private readonly ICommonRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<ProgramsController> _logger;

        public ProgramsController(ICommonRepository repository, IHostingEnvironment hostingEnvironment, ILogger<ProgramsController> logger)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }



        [HttpGet("user/{userId}", Name = "GetPrograms")]
        public async Task<IActionResult> GetPrograms([FromRoute]int? userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var programs = await _repository.GetProgramsFromUser(userId);

            if (programs == null)
            {
                return NotFound("Programs not found");
            }

            var mappedPrograms = Mapper.Map<List<WorkoutProgramDTO>>(programs);
            var programsList = programs.ToList();

            //add the pllaned dates to each Routine
            for (int i = 0; i < mappedPrograms.Count; i++) 
            {
                var mappedProgram = mappedPrograms[i];
                var program = programsList[i];
                mappedProgram.WorkoutRoutines
                                .ToList()
                                .ForEach(item => item.PlannedDates = program.RoutineProgramEntries
                                                                                .FirstOrDefault(r => r.WorkoutRoutineID == item.ID)?.PlannedDates);
            }


            return Ok(mappedPrograms);
        }


        [HttpGet("user/{userId}/program/{programId}", Name = "GetProgram")]
        public async Task<IActionResult> GetProgram([FromRoute]int? userId, [FromRoute]int? programId)
        {
            if (programId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var program = await _repository.GetProgram(userId, programId);

            if (program == null)
            {
                return NotFound("Program not found");
            }

            var mappedProgram = Mapper.Map<WorkoutProgramDTO>(program);
            //add the pllaned dates to each Routine
            mappedProgram.WorkoutRoutines
                        .ToList()
                        .ForEach(item => item.PlannedDates = program.RoutineProgramEntries
                                                                        .FirstOrDefault(r => r.WorkoutRoutineID == item.ID)?.PlannedDates);

            return Ok(mappedProgram);
        }


        [HttpPost("user/{userId}", Name = "PostProgram")]
        public async Task<IActionResult> PostProgram([FromRoute]int? userId, [FromBody] WorkoutProgramDTO programDTO)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var programEntity = Mapper.Map<WorkoutProgram>(programDTO);
            //add the pllaned dates to each RoutineProgramEntry
            programEntity.RoutineProgramEntries
                        .ToList()
                        .ForEach(item => item.PlannedDates = programDTO.WorkoutRoutines
                                                                        .FirstOrDefault(r => r.ID == item.WorkoutRoutineID)?.PlannedDates);

            var result = await _repository.AddProgramForUser(userId, programEntity);

            if (result == false)
            {
                return BadRequest("Add Failed for Prgram...");
            }

            if (programEntity.ImageBytes != null && programEntity.ImageBytes.Length != 0)
            {
                var imageResult = await SaveProgramImage(programEntity);
            }


            var programToReturn = Mapper.Map<WorkoutProgramDTO>(programEntity);

            return CreatedAtRoute(
                            routeName: "GetProgram",
                            routeValues: new
                            {
                                userId = userId,
                                programId = programToReturn.ID
                            },
                            value: programToReturn);
        }


        [HttpPut("user/{userId}/program/{programId}", Name = "UpdateProgram")]
        public async Task<IActionResult> UpdateProgram([FromRoute] int? userId, [FromRoute] int? programId, [FromBody] WorkoutProgramDTO programDTO)
        {
            if (userId == null || programId == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            programDTO.WTUserID = userId;
            programDTO.ID = (int)programId;
            var programEntity = Mapper.Map<WorkoutProgram>(programDTO);
            //add the pllaned dates to each RoutineProgramEntry
            programEntity.RoutineProgramEntries
                        .ToList()
                        .ForEach(item => item.PlannedDates = programDTO.WorkoutRoutines
                                                                        .FirstOrDefault(r => r.ID == item.WorkoutRoutineID)?.PlannedDates);

            var result = await _repository.UpdateProgram(programEntity);

            if (result == false)
            {
                return BadRequest("Update failed for program...");
            }

            if (programEntity.ImageBytes != null && programEntity.ImageBytes.Length != 0)
            {
                var imageResult = await SaveProgramImage(programEntity);
            }


            return NoContent();
        }


        [HttpPost("user/{userId}/program/{programId}/routines", Name = "UpdateRoutinesForProgram")]
        public async Task<IActionResult> UpdateRoutinesForProgram([FromRoute]int? userId, [FromRoute] int? programId, [FromBody] List<WorkoutRoutineDTO> routines)
        {
            if (userId == null || programId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var routineProgramEntries = routines.Select(item => new RoutineProgramEntry
            {
                WorkoutRoutineID = item.ID,
                WorkoutProgramID = programId,
                PlannedDates = item.PlannedDates
            });

            var result = await _repository.UpdateRoutinesForProgram(userId, programId, routineProgramEntries.ToList());

            if (result == false)
            {
                return BadRequest("Update of Routines for Program Failed...");
            }


            return NoContent();
        }


        [HttpDelete("user/{userId}/program/{programId}", Name = "DeleteProgram")]
        public async Task<IActionResult> DeleteProgram([FromRoute] int? userId, [FromRoute] int? programId)
        {
            if (userId == null || programId == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var result = await _repository.DeleteProgram(userId, programId);

            if (result == false)
            {
                return NotFound();
            }

            RemoveProgramImage(userId, programId);

            return NoContent();
        }

        [HttpPost("user/{userId}/program/{programId}/activate", Name = "ActivateProgram")]
        public async Task<IActionResult> ActivateProgram([FromRoute] int? userId, [FromRoute] int? programId)
        {
            if (userId == null || programId == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            await _repository.DeactivateAllPrograms(userId);
            var result = await _repository.ActivateProgram(userId, programId);

            if (result == false)
            {
                return BadRequest("Activaton failed for program...");
            }

            return NoContent();
        }

        [HttpPost("user/{userId}/program/{programId}/deactivate", Name = "DeactivateProgram")]
        public async Task<IActionResult> DeactivateProgram([FromRoute] int? userId, [FromRoute] int? programId)
        {
            if (userId == null || programId == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var result = await _repository.DeactivateProgram(userId, programId);

            if (result == false)
            {
                return BadRequest("Deactivation failed for program...");
            }

            return NoContent();
        }

        private async Task<bool> SaveProgramImage(WorkoutProgram programEntity)
        {
            try
            {
                // get this environment's web root path (the path
                // from which static content, like an image, is served)
                var webRootPath = _hostingEnvironment.WebRootPath;
                string folderName = "Images/Programs/" + programEntity.WTUserID.ToString();
                string fullFolderPath = $"{webRootPath}/{folderName}/";

                // create path if not exists... write bytes and auto-close stream
                FileInfo file = new FileInfo(fullFolderPath);
                file.Directory.Create();

                // create the filename
                string imageName = programEntity.ImagePath;
                var imageExtension = imageName.Substring(imageName.LastIndexOf("."));

                string fileName = "program_" + programEntity.ID + "_" + Guid.NewGuid() + imageExtension;

                //delete previuos image
                string[] fileList = Directory.GetFiles(fullFolderPath, $"*program_{programEntity.ID}*");
                foreach (var fileToDelete in fileList)
                {
                    System.IO.File.Delete(fileToDelete);
                }

                // the full file path
                var filePath = Path.Combine($"{fullFolderPath}/{fileName}");

                System.IO.File.WriteAllBytes(filePath, programEntity.ImageBytes);

                // fill out the filename
                programEntity.ImagePath = folderName + "/" + fileName;

                await _repository.UpdateImageForProgram(programEntity.ID, programEntity.ImagePath);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, ex.Message);
                return false;
            }

        }

        private bool RemoveProgramImage(int? userId, int? programId)
        {
            try
            {
                // get this environment's web root path (the path
                // from which static content, like an image, is served)
                var webRootPath = _hostingEnvironment.WebRootPath;
                string folderName = "Images/Programs/" + userId;
                string fullFolderPath = $"{webRootPath}/{folderName}/";

                // create path if not exists... write bytes and auto-close stream
                FileInfo file = new FileInfo(fullFolderPath);
                file.Directory.Create();

                //delete previuos image
                string[] fileList = Directory.GetFiles(fullFolderPath, $"*program_{programId}*");
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