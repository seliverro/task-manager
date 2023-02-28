using Microsoft.EntityFrameworkCore;
using TodoList;
using TodoList.Postgre;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Const.MY_ALLOW_SPECIFIC_ORIGINS,
        builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoListDataContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("TodoList"))
);

builder.Services.AddScoped<TaskService>(); // we don't use an interface here, currently there is no need in it 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(Const.MY_ALLOW_SPECIFIC_ORIGINS);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    scope.ServiceProvider.GetService<TodoListDataContext>()?.Database.Migrate();
}

app.Run();