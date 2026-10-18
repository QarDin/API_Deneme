using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ApiOrnek.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Get_Is_Work()
    {
        HttpClient client = new WebApplicationFactory<Program>().CreateClient();

        var response = await client.GetAsync("/notlar");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
    [Fact]
    public async Task Get_Id_Is_Work()
    {
        HttpClient client = new WebApplicationFactory<Program>().CreateClient();

        var response = await client.GetAsync("/notlar/3");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_Is_Work()
    {
        HttpClient client = new

        WebApplicationFactory<Program>().CreateClient();
        var responsePost = await client.PostAsJsonAsync("/notlar", new { Metin = "Yeni Metin Deneme" });
        var degisken = await responsePost.Content.ReadFromJsonAsync<Not>();
        var response = await client.PutAsJsonAsync($"/notlar/{degisken.Id}", new { Metin = "Guncellendi" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task Post_Is_Work()
    {
        HttpClient client = new WebApplicationFactory<Program>().CreateClient();

        var response = await client.PostAsJsonAsync("/notlar", new { Metin = "Test" });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    [Fact]
    public async Task Delete_Is_Work()
    {
        HttpClient client = new WebApplicationFactory<Program>().CreateClient();
        var degiskenResponse = await client.PostAsJsonAsync("/notlar", new { Metin = "Deneme Metini" });
        var result = await degiskenResponse.Content.ReadFromJsonAsync<Not>();
        var response = await client.DeleteAsync($"/notlar/{result.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
