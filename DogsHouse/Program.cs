using Microsoft.EntityFrameworkCore;
using DAL.Context;
using BLL.Interfaces;
using BLL.Services;
using System.Threading.RateLimiting;
using DogsHouse.Options;
using DAL.Repository;
using DAL.Repository.Interfaces;
using DAL.Repository.UnitOfWork;


var builder = WebApplication.CreateBuilder(args);


var rateOptions = builder.Configuration.GetSection("RateLimiting")
.Get<RateLimitingOptions>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connectionString = builder.Configuration.GetConnectionString("DataBase");


ConfiguarationService(builder.Services);


var app = builder.Build();



app.UseRateLimiter();


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


void ConfiguarationService(IServiceCollection services)
{
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
    builder.Services.AddDbContext<DogsHouseDbContext>(x => x.UseSqlServer(connectionString));
    builder.Services.AddTransient<DogsRepository>();
    builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
    builder.Services.AddTransient<IDogsService, DogsService>();
}