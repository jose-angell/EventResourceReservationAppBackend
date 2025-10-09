using EventResourceReservationApp.Api;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Infrastructure;
using EventResourceReservationApp.Infrastructure.Data;
using EventResourceReservationApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddApplicationServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// --- INICIO DEL SEEDING ---
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        await DbInitializer.SeedRolesAsync(serviceProvider);

    }
    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
// --- FIN DEL SEEDING ---

app.UseSerilogRequestLogging();
//Captura las excepciones no manejadas y las redirige a un middleware de manejo de excepciones
app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext context) =>
{
    var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
    //TODO: Logear la excepcion
    return Results.Problem(
        statusCode: StatusCodes.Status500InternalServerError,
        title: "Error inesperado del servidor",
        detail: exceptionHandler?.Error.Message);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
