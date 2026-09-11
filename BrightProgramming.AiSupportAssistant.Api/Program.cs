using BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOptions(builder.Configuration);
builder.Services.AddMappers();
builder.Services.AddAiProviders();
builder.Services.AddKnowledgeProviders();
builder.Services.AddApplicationServices();
builder.Services.AddExceptionHandling();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
