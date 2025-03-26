using GS1Takehome.Models;
using GS1Takehome.Models.Services;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
GlobalConfiguration.Configuration.UseInMemoryStorage();
builder.Services.AddHangfire(config => config.UseInMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add dependencies
builder.Services.AddScoped<IDataReceiverService, MockRetailerService>();
builder.Services.AddScoped<PriceModel, PriceModel>();

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
app.MapHangfireDashboard();

app.Run();
