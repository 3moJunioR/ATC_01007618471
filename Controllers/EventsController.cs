using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EventBookingAPI.Models;
using EventBookingAPI.Data;

namespace EventBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _ctx;

        public EventsController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetEvents()
        {
            return await _ctx.Events.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var e = await _ctx.Events.FindAsync(id);

            if (e == null)
                return NotFound();

            return e;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event ev)
        {
            _ctx.Events.Add(ev);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvent), new { id = ev.Id }, ev);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event ev)
        {
            if (id != ev.Id)
                return BadRequest();

            ev.UpdatedAt = DateTime.UtcNow;
            _ctx.Entry(ev).State = EntityState.Modified;

            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                if (!_ctx.Events.Any(x => x.Id == id))
                    return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _ctx.Events.FindAsync(id);
            if (ev == null)
                return NotFound();

            _ctx.Events.Remove(ev);
            await _ctx.SaveChangesAsync();

            return NoContent();
        }
    }
}