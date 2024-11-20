using BookLibrary.Authentication;
using BookLibrary.DataLayer;
using BookLibrary.IoC;
using BookLibrary.Middleware;

var builder = WebApplication.CreateBuilder(args);

// For now support only console logging (visible in docker console)
// Can be enhanced with LogStash/etc
builder.Logging.ClearProviders().AddConsole();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.RegisterServices(builder.Configuration);

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication()
    .AddScheme<NoTokenAuthSchemeOptions, NoTokenAuthSchemeHandler>(
        NoTokenAuthSchemeHandler.NoTokenAuth,
        opts => { }
    );

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler(); // Should be always in first place 

// Ensure the database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<IBookLibraryContext>();
    // Create the database if it doesn't exist
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
