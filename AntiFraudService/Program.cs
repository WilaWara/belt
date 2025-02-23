using AntiFraudService.Handlers;
using AntiFraudService.Kafka;
using AntiFraudService.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

var kafkaConsumer = new KafkaConsumer("transactions-topic", new TransactionHandler(new AntiFraudValidator()));
Task.Run(() => kafkaConsumer.Consume());

app.Run();
