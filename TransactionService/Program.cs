using Microsoft.EntityFrameworkCore;
using TransactionService.Core.Domain.Interfaces;
using TransactionService.Infrastructure.Comunicaction;
using TransactionService.Infrastructure.Repositories;
using AutoMapper;
using TransactionService.Presentation.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Agregando ruta de cadena de conexion
string connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<ApplicationDBContext>(
    options => options.UseNpgsql(connectionString)
);

// Add services to the container.
builder.Services.AddScoped<ITransactionService, TransactionService.Core.Application.TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddSingleton<ITransactionCommunicationProducer, TransactionCommunicationProducer>();
builder.Services.AddHostedService(provider =>
{
    var topic = "validation-results-topic"; // Nombre del tópico
    return new TransactionCommunicationConsumer(topic, provider); // Pasa el proveedor de servicios
});

builder.Services.AddAutoMapper(typeof(TransactionMapper)); // Para el Automapper


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
