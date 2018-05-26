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
    [Route("api/Programs")]
    public class ProgramsController : Controller
    {
        private readonly ICommonRepository _repository;

        public ProgramsController(ICommonRepository repository)
        {
            _repository = repository;
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

            var mappedPrograms = Mapper.Map<IEnumerable<WorkoutProgramDTO>>(programs);

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

            var program = await _repository.GetProgram(userId,programId);

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


    }
}