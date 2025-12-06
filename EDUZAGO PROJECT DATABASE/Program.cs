using Microsoft.EntityFrameworkCore;
using EDUZAGO_PROJECT_DATABASE.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
// builder.Services.AddDbContext<EduzagoContext>(options =>
//     options.UseInMemoryDatabase("EduzagoDb"));

var app = builder.Build();

// Seed Data
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var context = services.GetRequiredService<EduzagoContext>();
//     // EDUZAGO_PROJECT_DATABASE.Data.DbInitializer.Initialize(context);
// }

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
