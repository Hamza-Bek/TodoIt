using Domain.Enums;

namespace Domain.Models
{
    public class Todo : EntityBase
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Priority Priority { get; set; }
        public bool Completed { get; set; }
        public bool Pinned { get; set; }
        public DateTime DueDate { get; set; }
        public bool Overdue { get; set; }
        public Guid OwnerId { get; set; }
        public ApplicationUser? Owner { get; set; }
    }
}
