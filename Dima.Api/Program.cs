using Dima.Api;
using Dima.Api.Common.Api;
using Dima.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddDocumentation();
builder.AddSecurity();
builder.AddDataContexts();
builder.AddServices();
builder.AddCrossOrigin();

var app = builder.Build();

if(app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.UseCors(ApiConfiguration.CorsPolicyName);
app.AddSecurity();
app.MapEndpoints();
app.Run();
