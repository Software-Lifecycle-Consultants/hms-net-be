using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HMS.Models;
using HMS.Services.Repository_Service;
using HMS.DTOs;
using AutoMapper;

namespace HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly HMSDBContext _dbContext;
        private readonly ILogger<ContactsController> _logger;
        private readonly IRepositoryService<Contact> _repositoryService;
        private readonly IMapper _mapper;

        public ContactsController(HMSDBContext context, ILogger<ContactsController> logger, IRepositoryService<Contact> repositoryService, IMapper mapper)
        {
            _dbContext = context;
            _logger = logger;
            _repositoryService = repositoryService;
            _mapper = mapper;
        }


        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            _logger.LogInformation("GetContacts");
            return Ok(await _repositoryService.GetAllAsync());
          
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _repositoryService.GetByIdAsync(id);
           

            if (contact == null)
            {
                return NotFound();
            }
            
            return contact;
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }
            _repositoryService.Update(contact);
                     
            try
            {
                await _repositoryService.SaveAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await _repositoryService.ItemExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError("Error at PutContact {ex}", ex.Message);
                    
                }
            }

            return NoContent();
        }

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(ContactDTO contactDto)
        {

            Contact contact = _mapper.Map<Contact>(contactDto);          

            
            await _repositoryService.InsertAsync(contact);         

            return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _repositoryService.GetByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            await _repositoryService.DeleteAsync(contact);        
            return NoContent();
        }

        //private bool ContactExists(int id)
        //{
        //    return _dbContext.Contacts.Any(e => e.Id == id);
        //}
    }
}
