using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Pages;
using IMS.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();  // Add session support

builder.Services.AddHttpContextAccessor(); // Context accessor for authorization
builder.Services.AddHttpClient(); // Ensure HttpClientFactory is available

builder.Services.AddControllersWithViews(options => {
  options.Filters.Add(new LoginAuthorizationFilter()); // Add the login authorization filter globally
  options.Filters.Add(new ITAuthorizationFilter());  // Add the IT authorization filter globally
});

// ✅ Register DbContext as Factory (Fix for Concurrency Issues)
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment()) {
  app.UseExceptionHandler("/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.MapGet("/",context => {
  context.Response.Redirect("/Login");
  return Task.CompletedTask;
});

app.Run();
