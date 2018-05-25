using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WT_WebAPI.Common;
using WT_WebAPI.Entities.DTO;
using WT_WebAPI.Entities.DTO.WorkoutAssets;
using WT_WebAPI.Entities.WorkoutAssets;
using WT_WebAPI.Repository.Interfaces;


namespace WT_WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Sessions")]
    public class SessionsController : Controller
    {
        private readonly ICommonRepository _repository;

        public SessionsController(ICommonRepository repository)
        {
            _repository = repository;
        }


        [HttpPost("user/{userId}/Sessions", Name = "GetSessions")]
        public async Task<IActionResult> GetSessions([FromRoute]int? userId, [FromBody] WorkoutSessionRequest sessionRequest)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var sessions = await _repository.GetSessionsForUser(userId, sessionRequest.StartDate, sessionRequest.EndDate);

            if (sessions == null)
            {
                return NotFound("Sessions not found");
            }

            var mappedSessions = Mapper.Map<IEnumerable<WorkoutSessionDTO>>(sessions);

            return Ok(mappedSessions);
        }


        [HttpGet("user/{userId}/session/{sessionId}", Name = "GetSession")]
        public async Task<IActionResult> GetSession([FromRoute]int? userId, [FromRoute]int? sessionId)
        {
            if (sessionId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var session = await _repository.GetSession(userId, sessionId);

            if (session == null)
            {
                return NotFound("Session not found");
            }

            var mappedSession = Mapper.Map<WorkoutSessionDTO>(session);

            return Ok(mappedSession);
        }

        [HttpPost("user/{userId}/SessionForDay")]
        public async Task<IActionResult> GetSessionForDay([FromRoute]int? userId, [FromBody]WorkoutSessionRequest sessionRequest)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid || sessionRequest.CurrentDate.HasValue == false)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var session = await _repository.GetSessionForDay(userId, sessionRequest.CurrentDate.Value);

            if (session == null)
            {
                return NotFound("Session not found");
            }

            var mappedSession = Mapper.Map<WorkoutSessionDTO>(session);

            return Ok(mappedSession);
        }


        [HttpPost("user/{userId}/CreateOrUpdate", Name = "CreateOrUpdateSession")]
        public async Task<IActionResult> CreateOrUpdateSession([FromRoute]int? userId, [FromBody] WorkoutSessionRequest sessionRequest)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid || sessionRequest.CurrentDate.HasValue == false)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var routines = Mapper.Map<List<WorkoutRoutine>>(sessionRequest.Routines);
            var exercises = Mapper.Map<List<Exercise>>(sessionRequest.Exercises);
            var concreteExercises = Mapper.Map<List<ConcreteExercise>>(sessionRequest.ConcreteExercises);

            var session = await _repository.AddOrUpdateSession(userId,
                                                               sessionRequest.CurrentDate.Value,
                                                               routines,
                                                               exercises,
                                                               concreteExercises);

            if (session == null)
            {
                return BadRequest("Add or udpate Failed for Session...");
            }

            var sessionToReturn = Mapper.Map<WorkoutSessionDTO>(session);

            return CreatedAtRoute(
                            routeName: "GetSession",
                            routeValues: new
                            {
                                userId = userId,
                                sessionId = sessionToReturn.ID
                            },
                            value: sessionToReturn);
        }


        [HttpPut("user/{userId}/session/{sessionId}/UpdateConcreteExercises", Name = "UpdateConcreteExercises")]
        public async Task<IActionResult> UpdateConcreteExercises([FromRoute] int? userId,[FromRoute] int? sessionId, [FromBody] WorkoutSessionRequest sessionRequest)
        {
            if (userId == null || sessionId == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var concreteExercises = Mapper.Map<List<ConcreteExercise>>(sessionRequest.ConcreteExercises);

            var result = await _repository.UpdateConcreteExercises(userId, sessionId, concreteExercises);

            if (result == false)
            {
                return BadRequest("Update failed for concrete exercises ...");
            }

            return NoContent();
        }


        [HttpDelete("user/{userId}/session/{sessionId}/DeleteConcreteExercises")]
        public async Task<IActionResult> DeleteConcreteExercises([FromRoute] int? userId, [FromRoute] int? sessionId, [FromBody] List<int> concreteExerciseIds)
        {
            if (userId == null || sessionId == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var result = await _repository.DeleteConcreteExercises(userId, sessionId,concreteExerciseIds);

            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}