using Couchbase.Extensions.DependencyInjection;
using MT.NoSql.API.DAL.Concrete;
using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Settings;
using Neo4jClient;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCouchbase(options =>
{
    options.ConnectionString = builder.Configuration["Couchbase:ConnectionString"]; // Replace with your Couchbase Server connection string
    options.Username = builder.Configuration["Couchbase:UserName"];
    options.Password = builder.Configuration["Couchbase:Password"];
});

builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
var client = new GraphClient(new Uri(builder.Configuration["Neo4JConn:Url"]), builder.Configuration["Neo4JConn:UserName"], builder.Configuration["Neo4JConn:Password"]);
await client.ConnectAsync();
builder.Services.AddSingleton<IGraphClient>(client);

builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("DockerRedisConnection")));



builder.Services.Configure<MongoDbSettings>(
builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
