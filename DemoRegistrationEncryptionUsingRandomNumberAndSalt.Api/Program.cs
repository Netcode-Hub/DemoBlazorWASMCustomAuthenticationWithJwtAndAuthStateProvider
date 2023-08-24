using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Data;
using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Models;
using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Database connection string 'DefaultConnection' is not found"));
});
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection(nameof(TokenSettings)));
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "blazorCors",
        policy =>
        {
            policy.WithOrigins("https://localhost:7208")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("blazorCors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
