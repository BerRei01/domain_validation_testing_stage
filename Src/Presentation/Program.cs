using System.Globalization;
using System.Reflection;
using Application.Common.Composers;
using Application.CreateMeeting.Abstraction;
using Domain.Errors;
using Domain.Extensions;
using ErrorOr;
using FluentValidation;
using Infrastructure.Common.Composer;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.RegisterApplication();
builder.Services.RegisterInfrastructure();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(CreateMeetingRequest))));
builder.Services.AddValidatorsFromAssembly(typeof(CreateMeetingValidator).Assembly, ServiceLifetime.Transient);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

LocalizedErrors.StaticLocalizer.UiCulture = CultureInfo.GetCultureInfo("de-DE"); // hardcoding culture here to avoid issues with non English or German setups
LocalizedErrors.LocalizedErrorLoader.LoadAll();

app.Run();
// Expose builder and app for testing
public partial class Program {} // Required for WebApplicationFactory<TEntryPoint>
