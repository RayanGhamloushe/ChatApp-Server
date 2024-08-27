//using ChatApplication.Hub;
//using ChatApplication.Interface;
//using ChatApplication.Implementations;
//using ChatApplication.Implementation;
using ChatApplication.DB;
//using ChatApplication.Repository;
using ChatApplication.Interfaces;
using ChatApplication.Implementations;
//using ChatApplication.Hub;

var builder = WebApplication.CreateBuilder(args);




Connection.connectionsString = builder.Configuration.GetConnectionString("ConnectionString");
// Add services to the container
builder.Services.AddControllers();

// Add services for Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IChatRepository, ChatRepository>();
// Add SignalR services
builder.Services.AddSignalR();

// Register repositories with dependency injection
//builder.Services.AddScoped<IChatRepository, ChatRepository >();
//builder.Services.AddSingleton<ChatImplementation>();

// Add CORS configuration before building the app
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5501")
                  .AllowAnyHeader()
                  .WithMethods("GET", "POST")
                  .SetIsOriginAllowed((host) => true)
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Adds HTTP Strict Transport Security
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
// UseCors must be called before MapHub.
app.UseCors(); // Apply CORS policy

//app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub"); // Map SignalR hubs

app.Run();
