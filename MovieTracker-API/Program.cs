using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using MovieTracker_API.Extensions;
using MovieTracker_API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Service Config
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(typeof(MyExceptionFilter));
});

/*
builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy)); // every endpoints requires authentication
});
*/

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieTracker_API v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy");

// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
