using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HMS.Models;
using HMS.Models.Admin;

namespace HMS.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomsController : ControllerBase
    {
        private readonly HMSDBContext _context;

        public AdminRoomsController(HMSDBContext context)
        {
            _context = context;
        }

        // GET: api/AdminRooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminRoom>>> GetAdminRooms()
        {
            return await _context.AdminRooms.ToListAsync();
        }

        // GET: api/AdminRooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminRoom>> GetAdminRoom(Guid id)
        {
            var adminRoom = await _context.AdminRooms.FindAsync(id);

            if (adminRoom == null)
            {
                return NotFound();
            }

            return adminRoom;
        }

        // PUT: api/AdminRooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminRoom(Guid id, AdminRoom adminRoom)
        {
            if (id != adminRoom.Id)
            {
                return BadRequest();
            }

            _context.Entry(adminRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminRoomExists(id))
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

        // POST: api/AdminRooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminRoom>> PostAdminRoom(AdminRoom adminRoom)
        {
            _context.AdminRooms.Add(adminRoom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdminRoom", new { id = adminRoom.Id }, adminRoom);
        }

        // DELETE: api/AdminRooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminRoom(Guid id)
        {
            var adminRoom = await _context.AdminRooms.FindAsync(id);
            if (adminRoom == null)
            {
                return NotFound();
            }

            _context.AdminRooms.Remove(adminRoom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdminRoomExists(Guid id)
        {
            return _context.AdminRooms.Any(e => e.Id == id);
        }
    }
}
