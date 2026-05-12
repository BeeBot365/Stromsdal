using StromsdalKok.Infrastructure.DI;
using StromsdalKok.Application.DI;
using StromsdalKok.Infrastructure.Data;
using StromsdalKokApi.Api.Endpoints;
var builder = WebApplication.CreateBuilder(args);

// SERVICES
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.AddAuth();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Strömsdalkök API",
        Version = "v1"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (app.Configuration["UseInMemory"] != "true")
            await DbSeeder.SeedAsync(context);
    }
}

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

// ── Endpoints ──
app.MapGroup("api")
   .MapAuthEndpoints()
   .MapTabletEndpoints();

app.Run();

