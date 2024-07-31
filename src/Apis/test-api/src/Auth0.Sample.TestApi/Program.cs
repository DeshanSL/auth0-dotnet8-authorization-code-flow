using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
   
}).AddJwtBearer(options =>
{
    options.Authority = "https://dev-ng43bulw2a5xgvts.us.auth0.com/";
    options.Audience = "https://apis.test.com/v1/";
    
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("ReadTest", policy => policy.RequireClaim("permissions", "read:sample-response"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("api/test",(HttpContext context) =>
{
    return new  { Message = $"Request Authorized. Hello {context.User.Claims.First(a => a.Type ==ClaimTypes.NameIdentifier).Value}" };

}).RequireAuthorization( policyNames:"ReadTest");

app.MapControllers();

app.Run();
