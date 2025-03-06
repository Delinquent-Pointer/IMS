using Microsoft.AspNetCore.Mvc;
using IMS.Data;
using IMS.Models;
using System.Linq;

namespace IMS.Controllers {
  [Route("api/events")]
  [ApiController]
  public class CalendarEventsController:ControllerBase {
    private readonly AppDbContext _context;

    public CalendarEventsController(AppDbContext context) {
      _context = context;
    }

    [HttpGet]
    public IActionResult GetEvents() {
      var events = _context.CalendarEvents
          .Select(e => new {
            title = e.Title,
            start = e.StartDate.ToString("yyyy-MM-dd"),
            end = e.EndDate.HasValue ? e.EndDate.Value.ToString("yyyy-MM-dd") : null
          })
          .ToList();

      return Ok(events);
    }
  }
}
