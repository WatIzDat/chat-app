using Application;
using Clerk.Net.DependencyInjection;
using Domain;
using Domain.Discussions;
using Domain.Messages;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scrutor;
using System.Security.Claims;
using Web.Api.Infrastructure;
using Web.Api.Notifications;
using Web.Api.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .Scan(
        selector => selector
            .FromAssemblies(
                InfrastructureAssemblyReference.Assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IConfigureOptions<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            .AddClasses(false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(DomainAssemblyReference.Assembly, ApplicationAssemblyReference.Assembly);
});

builder.Services.AddScoped<DiscussionService>();

builder.Services.AddScoped<IMessageNotifications, MessageNotifications>();

// Add services to the container.

builder
    .Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddClerkApiClient(options =>
{
    options.SecretKey = builder.Configuration["Clerk:SecretKey"]!;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Clerk:Authority"];

        options.TokenValidationParameters = new()
        {
            ValidateAudience = false,
            NameClaimType = ClaimTypes.NameIdentifier
        };

        options.Events = new JwtBearerEvents()
        {
            OnTokenValidated = context =>
            {
                string? azp = context.Principal?.FindFirstValue("azp");

                if (string.IsNullOrEmpty(azp) || azp != builder.Configuration["Clerk:AuthorizedParty"])
                {
                    context.Fail("AZP claim is invalid or missing.");
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSignalR();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using IServiceScope scope = app.Services.CreateScope();

    using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.Migrate();
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("chat-hub");

app.Run();

// Exposes this class for integration testing
public partial class Program { }
