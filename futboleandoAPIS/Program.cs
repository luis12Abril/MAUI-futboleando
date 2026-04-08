using futboleandoAPIS.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbA85d0bFutboleandobdContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("cn"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.//
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

// UseHttpsRedirection deshabilitado: en IIS/SmarterASP el HTTPS lo termina IIS,
// no Kestrel. Habilitarlo causa bucles de redirección en hosting out-of-process.
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Endpoint de diagnóstico - sin base de datos, confirma que el servidor responde
app.MapGet("/api/ping", () => Results.Ok(new { status = "API funcionando", hora = DateTime.Now, version = "net9.0" }));

app.Run();
