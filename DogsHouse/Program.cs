using Microsoft.EntityFrameworkCore;
using DogsHouse.Context;
using Microsoft.Data.SqlClient;
using DogsHouse.Interfaces;
using DogsHouse.Services;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using DogsHouse.Options;



//TODO
//4. Please implement logic that handles situations
//when there are too many incoming requests to the application,
//so those could not be handled. There should be a setting
//that says how many requests the service can handle, 
//for example, 10 requests per second.In case there are more 
//incoming requests than in configuration application should
//return HTTP status code "429TooManyRequests".n 


var builder = WebApplication.CreateBuilder(args);


var rateOptions = builder.Configuration.GetSection("RateLimiting")
.Get<RateLimitingOptions>();


builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = rateOptions.PermitLimit,
            Window = TimeSpan.FromSeconds(rateOptions.Window)
        });
    });
}
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connectionString = builder.Configuration.GetConnectionString("DataBase");


builder.Services.AddDbContext<DogsHouseDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddTransient<IDogsService, DogsService>();

var app = builder.Build();



app.UseRateLimiter();


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
