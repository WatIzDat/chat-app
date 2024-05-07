using Application;
using Clerk.Net.DependencyInjection;
using Domain;
using Domain.Discussions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scrutor;
using Web.Api.Infrastructure;

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

app.UseAuthorization();

app.MapControllers();

app.Run();

// Exposes this class for integration testing
public partial class Program { }
