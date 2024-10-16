namespace Notes.Models.Dto
{
    public record NoteDto(Guid Id, string Title, string Description, DateTime CreatedDate);
}
