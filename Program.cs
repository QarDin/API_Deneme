using ApiOrnek;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<NotServisi>();
builder.Services.AddDbContext<DbNot>(Options => Options.UseNpgsql(builder.Configuration.GetConnectionString("Db")));
var app = builder.Build();
// Artik kullanilmiyor cunku docker ile posgtres sql e gectigimiz icin calisma zamaninda hata almamak icin kapattik.
// using (var scope = app.Services.CreateScope())
// {
//     scope.ServiceProvider.GetRequiredService<DbNot>().Database.EnsureCreated();
// }

app.MapGet("/notlar", async (NotServisi NotServisi) => await NotServisi.Liste_Getir_Async());

app.MapGet("/notlar/{id}", (NotServisi NotServis, int id) =>
{
    // var result = dotlar.notlar.FirstOrDefault(n => n.Id == id);
    Not? result = NotServis.Id_Getir(id);
    return result == null ? Results.NotFound($"{id} id yook") : Results.Ok(result);
});


app.MapPost("/notlar", async (NotServisi notServisi, YeniNot yeniNot) =>
{

    var userNot = await notServisi.Not_EKle_Async(yeniNot);
    return Results.Created($"/notlar/{userNot.Id}", userNot);

});

app.MapPut("/notlar/{id}", async (NotServisi notServisi, YeniNot yeniNot, int id) =>
{
    var degisenUserNot = await notServisi.Not_Degistir_Async(id, yeniNot);
    if (degisenUserNot == null) return Results.NotFound($"Oyle bir mesaj yok {404}");
    return Results.Ok(degisenUserNot);
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

app.MapGet("/basliklar/{id}", async (DbNot Veri, int id) =>
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
public record YeniNot(string Metin, int? BaslikId);
record YeniBaslik(string Title);

