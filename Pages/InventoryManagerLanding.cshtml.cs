using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace IMS.Pages {
  public class InventoryManagerLandingModel:PageModel {
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly HttpClient _httpClient;

    public InventoryManagerLandingModel(IDbContextFactory<AppDbContext> dbContextFactory,IHttpClientFactory httpClientFactory) {
      _dbContextFactory = dbContextFactory;
      _httpClient = httpClientFactory.CreateClient();
    }

    public IList<Product> Products { get; set; } = new List<Product>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty]
    public string? NewNote { get; set; }

    public IList<Note> Notes { get; set; } = new List<Note>();
    public IList<CalendarEvent> Events { get; set; } = new List<CalendarEvent>();

    // Weather & Timezone Data
    public string? WeatherInfo { get; set; }
    public string? UserTimeZone { get; set; }

    public async Task OnGetAsync() {
      HandleRedirectErrors();

      // Execute methods sequentially to avoid concurrency issues
      await LoadProductsAsync();
      LoadDefaultNotes();
      await LoadCalendarEventsAsync();
      await LoadUserLocationAsync();
    }

    public async Task<IActionResult> OnPostLogout() {
      HttpContext.Session.Clear();
      return RedirectToPage("/Login");
    }

    public async Task<IActionResult> OnPostAsync() {
      if(!string.IsNullOrEmpty(NewNote)) {
        Notes.Add(new Note { Content = NewNote,Timestamp = DateTime.Now });
      }
      return RedirectToPage();
    }

    private void HandleRedirectErrors() {
      string? error = HttpContext.Session.GetString("ITRedirectError");
      if(!string.IsNullOrEmpty(error)) {
        ModelState.AddModelError(string.Empty,error);
        HttpContext.Session.Remove("ITRedirectError");
      }
    }

    private async Task LoadProductsAsync() {
      await using var context = await _dbContextFactory.CreateDbContextAsync();
      var query = context.Products.AsQueryable();
      if(!string.IsNullOrEmpty(SearchTerm)) {
        query = query.Where(p => EF.Functions.Like(p.Name,$"%{SearchTerm}%") ||
                                 EF.Functions.Like(p.Description,$"%{SearchTerm}%"));
      }
      Products = await query.ToListAsync();
    }

    private void LoadDefaultNotes() {
      if(Notes.Count == 0) {
        Notes.Add(new Note { Content = "Dashboard initialized.",Timestamp = DateTime.Now.AddMinutes(-30) });
        Notes.Add(new Note { Content = "Inventory levels checked.",Timestamp = DateTime.Now.AddMinutes(-10) });
      }
    }

    private async Task LoadCalendarEventsAsync() {
      await using var context = await _dbContextFactory.CreateDbContextAsync();
      Events = await context.CalendarEvents.ToListAsync();
    }

    private async Task LoadUserLocationAsync() {
      await using var context = await _dbContextFactory.CreateDbContextAsync();
      var userId = GetCurrentUserId();
      var userProfile = await context.UserProfile
                                      .Where(u => u.Account_Id == userId)
                                      .FirstOrDefaultAsync();

      if(userProfile != null) {
        string location = $"{userProfile.City}, {userProfile.State}";
        UserTimeZone = userProfile.TimeZone;

        await FetchWeatherData(location);
      }
    }

    private async Task FetchWeatherData(string location) {
      try {
        var response = await _httpClient.GetAsync($"https://api.weather.gov/points/{GetWeatherCoordinates(location)}");
        if(response.IsSuccessStatusCode) {
          var jsonResponse = await response.Content.ReadAsStringAsync();
          using JsonDocument doc = JsonDocument.Parse(jsonResponse);
          if(doc.RootElement.TryGetProperty("properties",out var properties) &&
              properties.TryGetProperty("forecast",out var forecastUrlElement)) {

            var forecastUrl = forecastUrlElement.GetString();
            var forecastResponse = await _httpClient.GetAsync(forecastUrl);
            if(forecastResponse.IsSuccessStatusCode) {
              var forecastJson = await forecastResponse.Content.ReadAsStringAsync();
              using JsonDocument forecastDoc = JsonDocument.Parse(forecastJson);
              if(forecastDoc.RootElement.TryGetProperty("properties",out var forecastProps) &&
                  forecastProps.TryGetProperty("periods",out var periods) &&
                  periods.GetArrayLength() > 0) {

                var forecast = periods[0];
                WeatherInfo = $"{forecast.GetProperty("name").GetString()}: " +
                              $"{forecast.GetProperty("temperature").GetInt32()}°F, " +
                              $"{forecast.GetProperty("shortForecast").GetString()}";
              }
            }
          }
        }
      } catch(Exception ex) {
        WeatherInfo = "Weather data unavailable.";
      }
    }

    private string GetWeatherCoordinates(string location) {
      return location switch {
        "Spokane, WA" => "47.6588,-117.4260",
        "Seattle, WA" => "47.6062,-122.3321",
        _ => "47.6588,-117.4260" // Default to Spokane, WA
      };
    }

    private int GetCurrentUserId() {
      // Dummy Implementation – Replace with actual user authentication logic
      return 1;
    }
  }

  public class Note {
    public string? Content { get; set; }
    public DateTime Timestamp { get; set; }
  }
}
