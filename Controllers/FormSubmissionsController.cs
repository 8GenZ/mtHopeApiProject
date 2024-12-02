using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mtHopeApiProject.Data;
using mtHopeApiProject.Models;

namespace mtHopeApiProject.Controllers
{
    [Authorize(Policy = "ApiScopePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class FormSubmissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FormSubmissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FormSubmissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormSubmission>>> GetFormSubmissions()
        {
            return await _context.FormSubmissions.ToListAsync();
        }

        // GET: api/FormSubmissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FormSubmission>> GetFormSubmission(int id)
        {
            var formSubmission = await _context.FormSubmissions.FindAsync(id);

            if (formSubmission == null)
            {
                return NotFound();
            }

            return formSubmission;
        }

        // PUT: api/FormSubmissions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFormSubmission(int id, FormSubmission formSubmission)
        {
            if (id != formSubmission.Id)
            {
                return BadRequest();
            }

            _context.Entry(formSubmission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormSubmissionExists(id))
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

        // POST: api/FormSubmissions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FormSubmission>> PostFormSubmission(FormSubmission formSubmission)
        {
            _context.FormSubmissions.Add(formSubmission);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFormSubmission", new { id = formSubmission.Id }, formSubmission);
        }

        // DELETE: api/FormSubmissions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormSubmission(int id)
        {
            var formSubmission = await _context.FormSubmissions.FindAsync(id);
            if (formSubmission == null)
            {
                return NotFound();
            }

            _context.FormSubmissions.Remove(formSubmission);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FormSubmissionExists(int id)
        {
            return _context.FormSubmissions.Any(e => e.Id == id);
        }
    }
}
