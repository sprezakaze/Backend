using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using webapi.Entities;
using webapi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ClothingContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ClothesService>();
builder.Services.AddScoped<MailService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "static")),
    RequestPath = "/static"
});


app.UseAuthorization();

app.MapControllers();

app.Run();
