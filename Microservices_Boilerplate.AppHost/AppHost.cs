var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var identityDb = postgres.AddDatabase("identity-db");

var identityService = builder.AddProject<Projects.IdentityService>("identity-service")
    .WithReference(identityDb)
    .WaitFor(identityDb);

builder.AddProject<Projects.ApiGateway>("api-gateway")
    .WithReference(identityService);

builder.Build().Run();
