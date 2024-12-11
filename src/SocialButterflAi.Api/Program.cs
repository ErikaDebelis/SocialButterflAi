using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialButterFlAi.Data.Identity;
using SocialButterFlAi.Data.Chat;
using SocialButterFlAi.Data.Analysis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// // setting global options
// GlobalFFOptions.Configure(new FFOptions { BinaryFolder = "./bin", TemporaryFilesFolder = "/tmp" });

// // or
// GlobalFFOptions.Configure(options => options.BinaryFolder = "./bin");

#region Database Configuration
var dbConnectionString = "Host=postgres.socialbutterflai;Port=5434;Database=CueCoach;Username=postgres;Password=postgres;Include Error Detail=true";

builder.Services.AddDbContextFactory<IdentityDbContext>(options =>
{
    options.UseNpgsql(dbConnectionString);
}, ServiceLifetime.Transient);

builder.Services.AddDbContext<ChatDbContext>(options =>
{
    options.UseNpgsql(dbConnectionString);
}, ServiceLifetime.Transient);

builder.Services.AddDbContext<AnalysisDbContext>(options =>
{
    options.UseNpgsql(dbConnectionString);
}, ServiceLifetime.Transient);

// Run Identity Migrations at Startup if Needed
var identityFactory = new IdentityDbContextFactory(dbConnectionString);
var identityContext = identityFactory.CreateDbContext(Array.Empty<string>());
await identityContext.Database.MigrateAsync();

var chatFactory = new ChatDbContextFactory(dbConnectionString);
var chatContext = chatFactory.CreateDbContext(Array.Empty<string>());
await chatContext.Database.MigrateAsync();

var analysisContextFactory = new AnalysisDbContextFactory(dbConnectionString);
var analysisContext = analysisContextFactory.CreateDbContext(Array.Empty<string>());
await analysisContext.Database.MigrateAsync();
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();