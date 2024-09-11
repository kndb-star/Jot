using JotBot;
using JotBot.Domain;
using JotBot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseInMemoryDatabase("NotesDb"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/notes", async ([FromServices] NotesDbContext dbContext) =>
{
    return await dbContext.Notes.AsNoTracking()
                                .Select(x => new Note(x.Id, x.Title, x.Content, x.CreatedAtUtc, x.LastUpdatedAtUtc.Value, x.Tags))
                                .ToListAsync();
})
.WithName("GetAllNotes")
.WithOpenApi();

app.MapPost("/notes", async ([FromBody] CreateNote createNote, [FromServices] NotesDbContext dbContext) =>
{
    NoteEntity newNote = new NoteEntity
    {
        Title = createNote.Title,
        Content = createNote.Content,
        CreatedAtUtc = DateTimeOffset.UtcNow,
        Tags = createNote.Tags
    };

    await dbContext.Notes.AddAsync(newNote);
    await dbContext.SaveChangesAsync();
})
.WithName("CreateNote")
.WithOpenApi();

app.MapPut("/notes/{id}", async ([FromRoute] int id, [FromBody] UpdateNote updateNote, [FromServices] NotesDbContext dbContext) =>
{
    NoteEntity noteToUpdate = await dbContext.Notes.FindAsync(id);
    if (noteToUpdate == null)
    {
        return Results.NotFound();
    }

    noteToUpdate.Title = updateNote.Title;
    noteToUpdate.Content = updateNote.Content;
    noteToUpdate.Tags = updateNote.Tags;

    await dbContext.SaveChangesAsync();

    return Results.Ok(updateNote);
})
.WithName("UpdateNote")
.WithOpenApi();

app.MapDelete("/notes/{id}", async ([FromRoute] int id, [FromServices] NotesDbContext dbContext) =>
{
    NoteEntity noteToDelete = await dbContext.Notes.FindAsync(id);
    if (noteToDelete == null)
    {
        return Results.NotFound();
    }

    dbContext.Remove(noteToDelete);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("DeleteNote")
.WithOpenApi();

app.Run();
