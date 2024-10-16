namespace Notes.Models
{
    public class Note
    {
        public Note(string title, string description)
        {
            Title = title;
            Description = description;
            CreatedDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
