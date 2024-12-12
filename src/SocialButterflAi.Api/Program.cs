using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SocialButterFlAi.Data.Chat;
using SocialButterFlAi.Data.Identity;
using SocialButterFlAi.Data.Analysis;
using SocialButterflAi.Services.CueCoach;
using SocialButterflAi.Services.Analysis;
using SocialButterflAi.Services.LLMIntegration.Claude;
using Microsoft.Extensions.Logging;
using Settings = SocialButterflAi.Models.Integration.Settings;
using SocialButterflAi.Services.LLMIntegration.OpenAi;
using Microsoft.AspNetCore.Hosting;

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

var appSettings = builder.Configuration.GetSection("Settings").Get<Settings>();

#region Database Configuration
//todo: pull this using configuration in appsettings.json and integrations models for the connection string
var postgresSettings = appSettings.Postgres;
var dbConnectionString = $"Host={postgresSettings.Host};Port={postgresSettings.Port};Database={postgresSettings.Database};Username={postgresSettings.Username};Password={postgresSettings.Password};Include Error Detail={postgresSettings.IncludeErrorDetail}";

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

#region Services
builder.Services.AddScoped<IAnalysisService>(x =>
	{
        var logFactory = LoggerFactory.Create(b =>
        {
            b.AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning)
            .AddConsole();
        });

        var claudeClient = new ClaudeClient(
            appSettings.Claude,
            logFactory.CreateLogger<ClaudeClient>()
        );

        var openaiClient = new OpenAiClient(
            appSettings.OpenAi,
            logFactory.CreateLogger<OpenAiClient>()
        );

		return new AnalysisService(
            openaiClient,
            claudeClient,
			x.GetRequiredService<AnalysisDbContext>(),
			x.GetRequiredService<ILogger<AnalysisService>>(),
            x.GetRequiredService<IWebHostEnvironment>(),
            appSettings.AnalysisSettings
		);
	}
);

builder.Services.AddScoped<ICueCoachService>(x =>
	{
		return new CueCoachService(
            x.GetRequiredService<IAnalysisService>(),
			x.GetRequiredService<IdentityDbContext>(),
			x.GetRequiredService<ChatDbContext>(),
			x.GetRequiredService<ILogger<CueCoachService>>()
		);
	}
);
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();