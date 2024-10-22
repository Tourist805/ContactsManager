using ContactsApi.Database;
using ContactsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly ContactsContext _context;

    public ContactsController(ContactsContext context)
    {
        _context = context;
    }

    // GET: api/contacts?page=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contact>>> GetContacts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var contacts = await _context.Contacts
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return Ok(contacts);
    }

    // GET: api/contacts/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);

        if (contact == null) return NotFound();

        return Ok(contact);
    }

    // POST: api/contacts
    [HttpPost]
    public async Task<ActionResult<Contact>> AddContact(Contact contact)
    {
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }

    // PUT: api/contacts/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, Contact contact)
    {
        if (id != contact.Id) return BadRequest();

        _context.Entry(contact).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/contacts/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null) return NotFound();

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
