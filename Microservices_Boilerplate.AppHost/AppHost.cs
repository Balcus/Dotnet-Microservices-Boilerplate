using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

// --- Services Dependencies --- //
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

var minio = builder.AddContainer("minio", "minio/minio")
    .WithArgs("server", "/data", "--console-address", ":9001")
    .WithVolume("minio-data", "/data")
    .WithEndpoint(9000, 9000, name: "api")
    .WithEndpoint(9001, 9001, name: "console", scheme: "http")
    .WithLifetime(ContainerLifetime.Persistent);

var minioApiEndpoint = minio.GetEndpoint("api");

// --- Service Databases --- //
var identityDb = postgres.AddDatabase("identity-db");
var accountDb = postgres.AddDatabase("account-db");
var feedDb = mongo.AddDatabase("feed-db");

// --- Service Registration --- //
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

var accountService = builder.AddProject<Projects.Account_API>("account-service")
    .WaitFor(minio)
    .WaitFor(rabbitMq)
    .WaitFor(accountDb)
    .WithReference(minioApiEndpoint)
    .WithReference(rabbitMq)
    .WithReference(accountDb);

var gateway = builder.AddProject<Projects.ApiGateway>("api-gateway")
    .WithReference(identityService);

scalar.WithApiReference(identityService)
    .WithApiReference(gateway);

builder.Build().Run();
