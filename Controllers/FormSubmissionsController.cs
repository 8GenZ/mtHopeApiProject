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
using mtHopeApiProject.Services.EmailService;

namespace mtHopeApiProject.Controllersa
{
    //[Authorize(Policy = "ApiScopePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class FormSubmissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public FormSubmissionsController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
            formSubmission.SubmissionDate = DateTime.Now;
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

            // Build email subject and message
            var subject = $"You've Got a Letter from {formSubmission.Name}- {formSubmission.Email}";
            var body = $"<h1> You've Got a Letter! </h1><p><strong>Recipient:</strong> {formSubmission.Recipient}</p> <p>{formSubmission.Message}</p>";

            // Send the email
            try
            {
                await _emailService.SendEmailAsync("jonachav6414@gmail.com", subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to send email.");
            }

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
