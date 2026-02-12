using RecipeMicroservice.Data;
using RecipeMicroservice.Repositoties;
using RecipeMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection Helper
builder.Services.AddScoped<MariaDbContext>();

// Repository
builder.Services.AddScoped<RecipeRepository>();
builder.Services.AddScoped<StatusEquipRepository>();
builder.Services.AddScoped<EquipRepository>();

// Services
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<StatusEquipServices>();
builder.Services.AddScoped<EquipService>();
builder.Services.AddScoped<WebSocketHandler>();

// 1. กำหนด Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.Map("/ws", async (HttpContext context, WebSocketHandler handler) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await handler.HandleAsync(context, webSocket);
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();