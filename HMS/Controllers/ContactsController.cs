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
        private readonly ILogger<ContactsController> _logger;
        private readonly IRepositoryService<Contact> _repositoryService;
        private readonly IMapper _mapper;

        public ContactsController( ILogger<ContactsController> logger, IRepositoryService<Contact> repositoryService, IMapper mapper)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _mapper = mapper;
        }


        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetContacts()
        {
            try
            {
                _logger.LogInformation("Fetching all contacts.");

                var contacts = await _repositoryService.GetAllAsync();

                if (contacts == null || !contacts.Any())
                {
                    _logger.LogWarning("No contacts found.");
                    return NotFound("No contacts available.");
                }

                var contactDTOs = contacts.Select(contact => _mapper.Map<ContactDTO>(contact));
                return Ok(contactDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all contacts.");
                return StatusCode(500, "An error occurred while retrieving contacts.");
            }
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDTO>> GetContact(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching contact by ID: {ContactId}", id);

                var contact = await _repositoryService.GetByIdAsync(id);

                if (contact == null)
                {
                    _logger.LogWarning("Contact with ID {ContactId} not found", id);
                    return NotFound("Contact not found.");
                }

                var contactDto = _mapper.Map<ContactDTO>(contact);
                return Ok(contactDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching contact by ID: {ContactId}", id);
                return StatusCode(500, "An error occurred while retrieving contact.");
            }
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(Guid id, ContactDTO contactDto)
        {
            try
            {
                _logger.LogInformation("Updating contact with ID: {ContactId}", id);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating contact with ID: {ContactId}", id);
                    return BadRequest(ModelState);
                }
               
                var existingContact = await _repositoryService.GetByIdAsync(id);
                if (existingContact == null)
                {
                    _logger.LogWarning("Contact with ID: {ContactId} not found for update.", id);
                    return NotFound($"No contact found with ID {id}.");
                }

                _mapper.Map(contactDto, existingContact);
                existingContact.Id = id; // Explicitly set the Id just to assert control over it.

                _repositoryService.Update(existingContact);
                await _repositoryService.SaveAsync();

                _logger.LogInformation("Contact with ID: {ContactId} updated successfully.", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when updating contact with ID: {ContactId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when updating contact with ID: {ContactId}", id);
                return StatusCode(500, "A database error occurred while deleting the contact.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating contact with ID: {ContactId}", id);
                return StatusCode(500, "An error occurred while updating the contact.");
            }
        }

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ContactDTO>> PostContact(ContactDTO contactDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new contact.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating a new contact");
                    return BadRequest(ModelState);
                }

                Contact contact = _mapper.Map<Contact>(contactDto);
                
                await _repositoryService.InsertAsync(contact);
                
                ContactDTO resultDto = _mapper.Map<ContactDTO>(contact);                
                _logger.LogInformation("Successfully created a new contact with ID: {ContactId}", contact.Id);

                return CreatedAtAction("GetContact", new { id = contact.Id }, resultDto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when creating a new contact.");
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                // Log database update exceptions
                _logger.LogError(ex, "Database update error occurred while creating a new contact.");
                return StatusCode(500, "A database error occurred while creating the contact.");
            }           
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while creating a new contact.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete contact with ID: {ContactId}", id);

                var contact = await _repositoryService.GetByIdAsync(id);
                if (contact == null)
                {
                    _logger.LogWarning("Contact with ID: {ContactId} not found", id);
                    return NotFound();
                }

                await _repositoryService.DeleteAsync(contact);
                _logger.LogInformation("Successfully deleted contact with ID: {ContactId}", id);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when deleting contact with ID: {ContactId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when deleting contact with ID: {ContactId}", id);
                return StatusCode(500, "A database error occurred while deleting the contact.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when deleting contact with ID: {ContactId}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

    }
}
