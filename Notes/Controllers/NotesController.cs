using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.Contracts;
using Notes.DataAccess;
using Notes.Models;
using Notes.Models.Dto;
using System.Linq.Expressions;

namespace Notes.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class NotesController : ControllerBase
    {
        private readonly NotesDbContext _dbContext;

        public NotesController(NotesDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody]GetNotesRequest request, CancellationToken cancellationToken)
        {
            var notesQuery = _dbContext.Notes
                .Where(n => !string.IsNullOrWhiteSpace(request.Search) &&
                            n.Title.ToLower().Contains(request.Search.ToLower()));

            Expression<Func<Note, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "date" => note => note.CreatedDate,
                "title" => note => note.Title,
                _ => note => note.Id
            };

            notesQuery = request.SortOrder == "decs"
                ? notesQuery.OrderByDescending(selectorKey)
                : notesQuery.OrderBy(selectorKey);

            var noteDtos = await notesQuery
                .Select(n => new NoteDto(n.Id, n.Title, n.Description, n.CreatedDate))
                .ToListAsync(cancellationToken);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] CreateNoteRequest request, CancellationToken cancellationToken)
        {
            var note = new Note(request.Title, request.Description);

            await _dbContext.Notes.AddAsync(note, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}
