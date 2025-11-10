using Microsoft.EntityFrameworkCore;
using DogsHouse.Context;
using Microsoft.Data.SqlClient;
using DogsHouse.Interfaces;
using DogsHouse.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connectionString = builder.Configuration.GetConnectionString("DataBase");


builder.Services.AddDbContext<DogsHouseDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddTransient<IDogsService, DogsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();


app.Run();
