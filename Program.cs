using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbNot>(Options => Options.UseSqlite("Data Source=notlar.db"));
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<DbNot>().Database.EnsureCreated();
}

app.MapGet("/notlar", (DbNot Dotlar) => Dotlar.notlar.ToList());

app.MapGet("/notlar/{id}", (DbNot dotlar, int id) =>
{
    var result = dotlar.notlar.FirstOrDefault(n => n.Id == id);
    return result == null ? Results.NotFound() : Results.Ok(result);
});


app.MapPost("/notlar", async (YeniNot gelen, DbNot Dotlar) =>
{

    var not = new Not { Metin = gelen.Metin };
    Dotlar.Add(not);
    Dotlar.SaveChanges();
    return Results.Created($"/notlar/{not.Id}", not);
});

app.MapPut("/notlar/{id}", async (YeniNot gelenNot, DbNot Dotlar, int id) =>
{
    var not = Dotlar.notlar.FirstOrDefault(n => n.Id == id);
    if (not == null)
    {
        return Results.NotFound();
    }
    not.Metin = gelenNot.Metin;
    await Dotlar.SaveChangesAsync();
    return Results.Ok(not);
});

app.MapDelete("/notlar/{id}", (DbNot Dotlar, int id) =>
{
    var silinen = Dotlar.notlar.FirstOrDefault(n => n.Id == id);
    if (silinen == null)
    {
        return Results.NotFound();
    }
    Dotlar.notlar.Remove(silinen);
    Dotlar.SaveChanges();
    return Results.NoContent();
});

app.Run();

public class Not
{
    public int Id { get; set; }
    public string Metin { get; set; } = string.Empty;
}
record YeniNot(string Metin);
