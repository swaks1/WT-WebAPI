using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WT_WebAPI.Entities;
using WT_WebAPI.Entities.DBContext;

namespace WT_WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/WTUsers")]
    public class WTUsersController : Controller
    {
        private readonly WorkoutTrackingDBContext _context;

        public WTUsersController(WorkoutTrackingDBContext context)
        {
            _context = context;
        }

        // GET: api/WTUsers
        [HttpGet]
        public IEnumerable<WTUser> GetUsers()
        {
            return _context.Users.Include(w=>w.Exercises);
        }

        // GET: api/WTUsers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWTUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wTUser = await _context.Users.SingleOrDefaultAsync(m => m.WTUserID == id);

            if (wTUser == null)
            {
                return NotFound();
            }

            return Ok(wTUser);
        }

        // PUT: api/WTUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWTUser([FromRoute] int id, [FromBody] WTUser wTUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wTUser.WTUserID)
            {
                return BadRequest();
            }

            _context.Entry(wTUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WTUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WTUsers
        [HttpPost]
        public async Task<IActionResult> PostWTUser([FromBody] WTUser wTUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Users.Add(wTUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWTUser", new { id = wTUser.WTUserID }, wTUser);
        }

        // DELETE: api/WTUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWTUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wTUser = await _context.Users.SingleOrDefaultAsync(m => m.WTUserID == id);
            if (wTUser == null)
            {
                return NotFound();
            }

            _context.Users.Remove(wTUser);
            await _context.SaveChangesAsync();

            return Ok(wTUser);
        }

        private bool WTUserExists(int id)
        {
            return _context.Users.Any(e => e.WTUserID == id);
        }
    }
}