using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Pages;
using IMS.Filters;
using IMS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();  // Add session support for session variables
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews(options => {
  options.Filters.Add(new LoginAuthorizationFilter());
  options.Filters.Add(new ITAuthorizationFilter());
});

// Add Database Context for Azure SQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add your custom user account service
builder.Services.AddScoped<UserAccountService>();

// Add authentication and cookie settings
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies",options => {
      options.LoginPath = "/Login";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment()) {
  app.UseExceptionHandler("/Error");
  app.UseHsts();
}

app.UseHttpsRedirection(); // docker container possible fix
// app.UseDeveloperExceptionPage();  // Optional
app.UseRouting();

app.UseAuthentication();  // correct order
app.UseAuthorization();   // correct order
app.UseSession();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.MapGet("/",context => {
  context.Response.Redirect("/Login");
  return Task.CompletedTask;
});

// app.Urls.Clear(); // do not use this without adding a new URL
// app.Urls.Add("http://0.0.0.0:80"); // possible fix for docker container

app.Run();
