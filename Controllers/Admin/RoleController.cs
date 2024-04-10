using HMS.DTOs;
using HMS.Models;
using HMS.Services.RoleManagerService;
using HMS.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HMS.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
       
        private HMSDBContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public RoleController(HMSDBContext dbContext,RoleManager<IdentityRole>  roleManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
        }
        // GET: api/<RoleController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetRoles()
        {
            try
            {
                var dbRoles = _roleManager.Roles;
                var roles =await dbRoles.ToListAsync();
                List<string> result = new List<string>();

                if (roles.Count>0)
                {
                    foreach (var role in roles)
                    {
                        if (!string.IsNullOrWhiteSpace(role.Name)) 
                            result.Add(role.Name);
                    }
                }
               
                return Ok(result);

            }
            catch (Exception)
            {

                throw;
            }
            

        }

        //// GET api/<RoleController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<RoleController>
        [CustomValidationFilter]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RoleDTO role)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(role.Name))
                {
                    return BadRequest("Role name cannot be empty");
                }
                else
                {
                    var roles = _roleManager.Roles;
                    IdentityResult result = new IdentityResult();
                    foreach (var roleName in roles.ToList())
                    {
                        if (!await _roleManager.RoleExistsAsync(role.Name))
                            result = await _roleManager.CreateAsync(new IdentityRole(role.Name));
                        else
                            return Conflict("Data already exists. Please provide unique data.");

                    }
                    if (result.Succeeded)
                        return Ok();
                    else return BadRequest($"Operation failed {result}");
                }

            }
            catch (Exception)
            {

                throw;
            }
          

        }

        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
