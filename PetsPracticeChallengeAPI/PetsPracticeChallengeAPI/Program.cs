using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var myPolicy = "_myPolicy";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddNewtonsoftJson();
// Inject DB Context
builder.Services.AddDbContext<DbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("PetsDbConnectionString")));


//services cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myPolicy,
                      policy =>
                      {
                          policy.WithOrigins().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                      });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app cors
app.UseCors(myPolicy);

app.UseHttpsRedirection();



app.UseAuthorization();

app.MapControllers();

app.Run();
