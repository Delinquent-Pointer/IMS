using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.Data;
using IMS.Models;
using Microsoft.Extensions.Logging;

namespace IMS.Controllers {
  [ApiController]
  [Route("api/calendar-events")] // ✅ Changed route to prevent conflicts
  public class EventsController:ControllerBase {
    private readonly AppDbContext _context;
    private readonly ILogger<EventsController> _logger;

    public EventsController(AppDbContext context,ILogger<EventsController> logger) {
      _context = context;
      _logger = logger;
    }

    /// <summary>
    /// Get all calendar events from the database
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetEvents() {
      try {
        var events = await _context.CalendarEvents
            .Select(e => new {
              id = e.Id,
              title = e.Title,
              start = e.StartDate,
              end = e.EndDate
            })
            .ToListAsync();

        return Ok(events);
      } catch(Exception ex) {
        _logger.LogError($"Error fetching events: {ex.Message}");
        return StatusCode(500,new { message = "An error occurred while fetching events.",error = ex.Message });
      }
    }

    /// <summary>
    /// Add a new event to the calendar
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddEvent([FromBody] CalendarEvent newEvent) {
      if(newEvent == null)
        return BadRequest(new { message = "Invalid event data." });

      if(string.IsNullOrWhiteSpace(newEvent.Title) || newEvent.StartDate == default)
        return BadRequest(new { message = "Title and Start Date are required." });

      try {
        _context.CalendarEvents.Add(newEvent);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Event added: {newEvent.Title} (ID: {newEvent.Id})");
        return Ok(new { message = "Event Added!",eventId = newEvent.Id });
      } catch(Exception ex) {
        _logger.LogError($"Error saving event: {ex.Message}");
        return StatusCode(500,new { message = "An error occurred while saving the event.",error = ex.Message });
      }
    }

    /// <summary>
    /// Delete an event by ID
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id) {
      var eventToDelete = await _context.CalendarEvents.FindAsync(id);

      if(eventToDelete == null)
        return NotFound(new { message = "Event not found." });

      try {
        _context.CalendarEvents.Remove(eventToDelete);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Event deleted: ID {id}");
        return Ok(new { message = "Event Deleted." });
      } catch(Exception ex) {
        _logger.LogError($"Error deleting event ID {id}: {ex.Message}");
        return StatusCode(500,new { message = "An error occurred while deleting the event.",error = ex.Message });
      }
    }
  }
}
