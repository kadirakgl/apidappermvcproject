var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<apidappermvcproje.Data.DapperContext>();
builder.Services.AddScoped<apidappermvcproje.Data.ProductRepository>();
builder.Services.AddScoped<apidappermvcproje.Data.CategoryRepository>();
builder.Services.AddScoped<apidappermvcproje.Data.CustomerRepository>();
builder.Services.AddScoped<apidappermvcproje.Data.OrderRepository>();
builder.Services.AddControllers(); // API controller'lar için eklendi

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers(); // API endpoint'leri için eklendi

app.Run();
