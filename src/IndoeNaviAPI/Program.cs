using IndoeNaviAPI.Models;
using IndoeNaviAPI.Services;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoClient>(new MongoClient(builder.Configuration.GetConnectionString("mongo_db")));
builder.Services.AddSingleton<IMongoDBService, MongoDBService>();
builder.Services.AddSingleton<IMapService, MapService>();
builder.Services.AddSingleton<IStatisticService, StatisticService>();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
	builder.AllowAnyOrigin()
		   .AllowAnyMethod()
		   .AllowAnyHeader();
}));

var app = builder.Build();


app.Services.GetService<IMongoDBService>().SetUniqueKey<Map>("maps", "{Area: 1}");
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();