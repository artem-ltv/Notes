using Notes.Models.Dto;

namespace Notes.Contracts
{
    public record GetNotesResponse(List<NoteDto> notes);
}
