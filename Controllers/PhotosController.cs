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
    public class PhotosController : ControllerBase
    {
        private readonly HMSDBContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _imageFileService;
        private readonly IRepositoryService<Photo> _photoRepository;


        public PhotosController(HMSDBContext context, IRepositoryService<Photo> repositoryService, IFileService fileService, IMapper mapper)
        {
            _context = context;
            _photoRepository = repositoryService;
            _imageFileService = fileService;
            _mapper = mapper;

        }

        // GET: api/Photos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos()
        {
            return await _context.Photos.ToListAsync();
        }

        // GET: api/Photos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Photo>> GetPhoto(Guid id)
        {
            var photo = await _context.Photos.FindAsync(id);

            if (photo == null)
            {
                return NotFound();
            }

            return photo;
        }

        // PUT: api/Photos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoto(Guid id, Photo photo)
        {
            if (id != photo.Id)
            {
                return BadRequest();
            }

            _context.Entry(photo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
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

        // POST: api/Photos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Photo>> PostPhoto(Photo photoDto)
        {
            try
            {
                Tuple<int, string> fileSaveResult;
                string? FilePath = default;

                if (photoDto.File != null)
                {
                    fileSaveResult = _imageFileService.SaveFile(photoDto.File);
                    if (fileSaveResult.Item1 == 1)
                    {
                        FilePath = fileSaveResult.Item2;
                    }
                    else
                    {
                        return BadRequest(fileSaveResult.Item2); // Return an error message if the file save failed
                    }
                }

                Photo photo = _mapper.Map<Photo>(photoDto);
                photo.FilePath = FilePath;

                await _photoRepository.InsertAsync(photo);
                return CreatedAtAction("GetPhoto", new { id = photo.Id }, photo);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                //_logger.LogError($"Exception at POST: {ex}");
                throw;
            }
        }


        // DELETE: api/Photos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(Guid id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhotoExists(Guid id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }
    }
}
