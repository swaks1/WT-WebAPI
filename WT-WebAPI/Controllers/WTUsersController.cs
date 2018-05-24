using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WT_WebAPI.Common;
using WT_WebAPI.Entities;
using WT_WebAPI.Entities.DTO;
using WT_WebAPI.Repository.Interfaces;

namespace WT_WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/WTUsers")]
    public class WTUsersController : Controller
    {
        private readonly ICommonRepository _repository;

        public WTUsersController(ICommonRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetWTUser([FromRoute] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var wTUser = await _repository.GetUserByUsername(username);

            if (wTUser == null)
            {
                return NotFound("User not found");
            }

            var mappedUser = Mapper.Map<WTUserDTO>(wTUser);

            return Ok(mappedUser);

        }


        [HttpGet("FullInfo/{username}")]
        public async Task<IActionResult> GetWTUserFullInfo([FromRoute] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wTUser = await _repository.GetUserByUsernameFullInfo(username);

            if (wTUser == null)
            {
                return NotFound();
            }

            var mappedUser = Mapper.Map<WTUserDTO>(wTUser);
            return Ok(mappedUser);

        }


        [HttpPut("{username}")]
        public async Task<IActionResult> PutWTUser([FromRoute] string username, [FromBody] WTUserDTO wTUser)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            wTUser.Username = username;
            var wtUserEntity = Mapper.Map<WTUser>(wTUser);
            var result = await _repository.UpdateUser(wtUserEntity);

            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteWTUser([FromRoute] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var result = await _repository.DeleteUser(username);

            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}