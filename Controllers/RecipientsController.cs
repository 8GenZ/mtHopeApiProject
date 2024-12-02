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
    public class RecipientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecipientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Recipients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipient>>> GetRecipients()
        {
            return await _context.Recipients.ToListAsync();
        }

        // GET: api/Recipients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipient>> GetRecipient(int id)
        {
            var recipient = await _context.Recipients.FindAsync(id);

            if (recipient == null)
            {
                return NotFound();
            }

            return recipient;
        }

        // PUT: api/Recipients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipient(int id, Recipient recipient)
        {
            if (id != recipient.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipientExists(id))
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

        // POST: api/Recipients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipient>> PostRecipient(Recipient recipient)
        {
            _context.Recipients.Add(recipient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipient", new { id = recipient.Id }, recipient);
        }

        // DELETE: api/Recipients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipient(int id)
        {
            var recipient = await _context.Recipients.FindAsync(id);
            if (recipient == null)
            {
                return NotFound();
            }

            _context.Recipients.Remove(recipient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipientExists(int id)
        {
            return _context.Recipients.Any(e => e.Id == id);
        }
    }
}
