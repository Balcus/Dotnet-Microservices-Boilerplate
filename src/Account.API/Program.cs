using Account.API.Consumers;
using Account.API.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedAbstractions.Implementations;
using SharedAbstractions.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<AccountDbContext>("account-db");

builder.Services.AddScoped<ICrudRepository<string, Account.API.Data.Entities.Account>>(provider =>
{
    var context = provider.GetRequiredService<AccountDbContext>();
    return new BaseCrudRepository<string, Account.API.Data.Entities.Account, AccountDbContext>(context);
});


builder.Services.AddMassTransit(config =>
{
    config.SetKebabCaseEndpointNameFormatter();
    config.AddConsumer<UserRegisteredEventConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("event-bus"));
        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();