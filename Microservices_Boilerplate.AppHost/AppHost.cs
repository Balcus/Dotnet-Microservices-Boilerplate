using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var mongo = builder.AddMongoDB("mongo")
    .WithMongoExpress()
    .WithLifetime(ContainerLifetime.Persistent);

var scalar = builder.AddScalarApiReference();

var rabbitMq = builder.AddRabbitMQ("event-bus")
    .WithManagementPlugin();

var identityDb = postgres.AddDatabase("identity-db");

var feedDb = mongo.AddDatabase("feed-db");

var identityService = builder.AddProject<Projects.Identity_API>("identity-service")
    .WaitFor(identityDb)
    .WaitFor(rabbitMq)
    .WithReference(identityDb)
    .WithReference(rabbitMq);

var feedService = builder.AddProject<Projects.Feed_API>("feed-service")
    .WaitFor(feedDb)
    .WaitFor(rabbitMq)
    .WithReference(feedDb)
    .WithReference(rabbitMq);

var gateway = builder.AddProject<Projects.ApiGateway>("api-gateway")
    .WithReference(identityService);

scalar.WithApiReference(identityService)
    .WithApiReference(gateway);

builder.Build().Run();
