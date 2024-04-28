using Microsoft.AspNetCore.Builder;
using System.Net.Http;
using tpmodul10_1302223075;
using static tpmodul10_1302223075.Mahasiswa;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DBMahasiswa>(opt => opt.UseInMemoryDatabase("mahasiswa"));
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

// configure swagger tutorial di https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio-code
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/", () =>
{
    // dari aspnetcore.httml
    return Results.Redirect("/swagger"); // redirect to swagger
});

app.MapGet("/mahasiswa", async (DBMahasiswa db) =>
{
    // default data using array list 
    var mhs = new Mahasiswa[]
    {
        new Mahasiswa { Id = 1, Nama = "Dara Sheiba Malika Choiriyyah", Nim = "" },
        new Mahasiswa { Id = 2, Nama = "Zabrina Virgie", Nim = "1302223055" },
        new Mahasiswa { Id = 3, Nama = "Nasya Kirana Marendra", Nim = "1302223148" },
        new Mahasiswa { Id = 4, Nama = "M Tsaqif Zayyan", Nim = "1302220141" },
        new Mahasiswa { Id = 5, Nama = "M Arifin Ilham", Nim = "1302223061" },
        new Mahasiswa { Id = 6, Nama = "Rafie Aydin Ihsan", Nim = "1302220065" },
    };
    await db.SaveChangesAsync();
    return Results.Ok(await db.mhs.ToListAsync());
});

app.MapGet("/mahasiswa/{id}", async (DBMahasiswa db, int id) =>
{
    var mhs = await db.mhs.FindAsync(id);
    if (mhs == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(mhs);
});

app.MapPost("/mahasiswa", async (DBMahasiswa db, Mahasiswa mhs) =>
{
    Console.WriteLine(mhs);
    db.mhs.Add(mhs);
    await db.SaveChangesAsync();
    return Results.Created($"/mahasiswa/{mhs.Id}", mhs);
});

app.MapPut("/mahasiswa/{id}", async (DBMahasiswa db, int id, Mahasiswa mhs) =>
{
    if (id != mhs.Id)
    {
        return Results.BadRequest();
    }
    db.Entry(mhs).State = EntityState.Modified;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/mahasiswa/{id}", async (DBMahasiswa db, int id) =>
{
    var mhs = await db.mhs.FindAsync(id);
    if (mhs == null)
    {
        return Results.NotFound();
    }
    db.mhs.Remove(mhs);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();