using RecipeMicroservice.Data;
using RecipeMicroservice.Repositoties;
using RecipeMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// DI 
builder.Services.AddScoped<MariaDbContext>();
builder.Services.AddScoped<RecipeRepository>();
builder.Services.AddScoped<StatusEquipRepository>();
builder.Services.AddScoped<EquipRepository>();
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<StatusEquipServices>();
builder.Services.AddScoped<EquipService>();
builder.Services.AddScoped<WebSocketHandler>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // สำหรับทดสอบใน Docker ให้ผ่านทุก Origin ก่อนได้
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// แสดง Swagger เสมอถ้าต้องการ Test บน Docker ง่ายๆ
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
});

// ย้าย Map WebSocket มาไว้ก่อน UseAuthorization (ถ้าไม่ได้ใช้ Auth กับ WS)
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

app.UseAuthorization();
app.MapControllers();
app.Run();