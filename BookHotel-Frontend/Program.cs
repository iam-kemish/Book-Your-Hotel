

using BookHotel_Frontend.Mapper;
using BookHotel_Frontend.Services;
using BookHotel_Frontend.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddHttpClient<IHotelService, HotelServiceClass>();
builder.Services.AddScoped<IHotelService,HotelServiceClass>();

builder.Services.AddHttpClient<IHotelNumberService, HotelNumberServiceClass>();
builder.Services.AddScoped<IHotelNumberService, HotelNumberServiceClass>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
