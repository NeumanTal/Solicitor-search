using InfoTrack.Application.Interfaces;
using InfoTrack.Application.Services;
using InfoTrack.Infrastructure.Parsers;
using InfoTrack.Infrastructure.Scrapers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISolicitorSearch, SolicitorSearch>();
builder.Services.AddScoped<ISolicitorParser, SolicitorParser>();
//builder.Services.AddScoped<ISolicitorScraper, MockSolicitorScraper>();
builder.Services.AddHttpClient<ISolicitorScraper, SolicitorScraper>(
    client =>
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Vue",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Vue");

app.MapControllers();

app.Run();