using ApiOrnek;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbNot>(Options => Options.UseNpgsql(builder.Configuration.GetConnectionString("Db")));
var app = builder.Build();
// Artik kullanilmiyor cunku docker ile posgtres sql e gectigimiz icin calisma zamaninda hata almamak icin kapattik.
// using (var scope = app.Services.CreateScope())
// {
//     scope.ServiceProvider.GetRequiredService<DbNot>().Database.EnsureCreated();
// }

app.MapGet("/notlar", (DbNot Dotlar) => Dotlar.notlar.ToList());

app.MapGet("/notlar/{id}", (DbNot dotlar, int id) =>
{
    var result = dotlar.notlar.FirstOrDefault(n => n.Id == id);
    return result == null ? Results.NotFound() : Results.Ok(result);
});


app.MapPost("/notlar", async (YeniNot gelen, DbNot Dotlar) =>
{

    var not = new Not { Text = gelen.Metin, BaslikId = gelen.BaslikId };
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
    not.Text = gelenNot.Metin;
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
app.MapPost("/basliklar", async (YeniBaslik Gelen, DbNot veri) =>
{
    var newTitle = new Baslik { Title = Gelen.Title };
    veri.Add(newTitle);
    await veri.SaveChangesAsync();
    return Results.Ok(newTitle.Id);

});

app.MapGet("basliklar/{id}", async (DbNot Veri, int id) =>
{
    var gelecekDeger = Veri.Basliklar.Include(b => b.Notes).FirstOrDefault(a => a.Id == id);
    if (gelecekDeger == null) return Results.NotFound(id);
    return Results.Ok(gelecekDeger);
});
app.Run();

public class Not
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int? BaslikId { get; set; }
    [JsonIgnore]
    public Baslik? Baslik { get; set; } = null!;

}
record YeniNot(string Metin, int? BaslikId);
record YeniBaslik(string Title);

