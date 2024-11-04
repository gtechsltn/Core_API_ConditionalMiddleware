using Core_API_ConditionalMiddleware.Middlewares;
using Core_API_ConditionalMiddleware.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<WowdbContext>(options => { 
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// To Read the HTTP Body
app.Use((context, next) =>
{
    context.Request.EnableBuffering();

    return next();
});

// Use the RequestLoggerMiddleware Conditionally
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api") && (
     context.Request.Method == "POST" || context.Request.Method == "PUT" || context.Request.Method == "DELETE"
), appBuilder =>
{
    appBuilder.UseMiddleware<RequestLoggerMiddleware>();
});

app.MapGet("/api/expenses", async (WowdbContext ctx) => {
    return await ctx.Expenses.ToListAsync();
});

app.MapPost("/api/expenses", async (HttpContext context, WowdbContext ctx, Expense expense) =>
{
    await ctx.Expenses.AddAsync(expense);
    await ctx.SaveChangesAsync();
    return Results.Created($"/api/{expense.ExpensesId}", expense);
});

app.MapPut("/api/expenses/{id}", async (WowdbContext ctx, int id, Expense expense) =>
{
    if (Convert.ToInt32(id) != expense.ExpensesId)
    {
        return Results.BadRequest();
    }

    ctx.Entry(expense).State = EntityState.Modified;
    await ctx.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/expenses/{id}", async (WowdbContext ctx, int id) =>
{
    var expense = await ctx.Expenses.FindAsync(Convert.ToDouble(id));
    if (expense == null)
    {
        return Results.NotFound();
    }

    ctx.Expenses.Remove(expense);
    await ctx.SaveChangesAsync();
    return Results.NoContent();
});



app.Run();

 