using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HMS.Models;
using HMS.Services.FileService;
using HMS.DTOs;
using AutoMapper;
using HMS.Services.Repository_Service;

namespace HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        //private readonly ILogger _logger;
        private readonly IFileService _imageFileService;
        private readonly IRepositoryService<Room> _roomRepositoryService;
        private readonly HMSDBContext _context;
        private readonly IMapper _mapper;

        public RoomsController(HMSDBContext context,IRepositoryService<Room> repositoryService, IFileService fileService, IMapper mapper)
        {
            _context = context;
            _roomRepositoryService = repositoryService;
            _imageFileService = fileService;
            _mapper = mapper;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return Ok(await _roomRepositoryService.GetAllAsync());         
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _roomRepositoryService.GetByIdAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        // PUT: api/Rooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.Id)
            {
                return BadRequest();
            }

            _roomRepositoryService.Update(room);
           
            try
            {
                await _roomRepositoryService.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _roomRepositoryService.ItemExistsAsync(id))
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

        // POST: api/Rooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom([FromForm]RoomDTO roomDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                else 
                {
                    Tuple<int,string> fileSaveResult;
                    string? coverImagePath = default;
                    if (roomDto.ImageFile != null)
                    {
                        fileSaveResult = _imageFileService.SaveFile(roomDto.ImageFile);
                        if (fileSaveResult.Item1 == 1)
                            coverImagePath = fileSaveResult.Item2;
                    }
                                          
                    Room room = _mapper.Map<Room>(roomDto);
                    room.CoverImagePath = coverImagePath;                

                    await _roomRepositoryService.InsertAsync(room);
                    return CreatedAtAction("GetRoom", new { id = room.Id }, room);
                }
                

            }
            catch (Exception ex)
            {

               // _logger.LogError($"Exception at POST: {ex}");
                throw;
            }
            
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _roomRepositoryService.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            await _roomRepositoryService.DeleteAsync(room); 
            return NoContent();
        }

    }
}
