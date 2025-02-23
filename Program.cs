using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Pages;  // Correct namespace for AppDbContext

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add Database Context for Azure SQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment()) {
  app.UseExceptionHandler("/Error");
  app.UseHsts(); // The default HSTS value is 30 days
}

app.UseHttpsRedirection();
// app.UseDeveloperExceptionPage();  // Add this line to see detailed error messages
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.MapGet("/", context => {
    context.Response.Redirect("/Login");
    return Task.CompletedTask;
});

app.Run();
