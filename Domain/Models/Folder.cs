namespace Domain.Models
{
    public class Folder : EntityBase
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<Note>? Notes { get; set; }
        public Guid OwnerId { get; set; }
        public ApplicationUser? Owner { get; set; }
    }
}
