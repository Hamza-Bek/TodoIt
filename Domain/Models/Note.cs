using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Note
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsPined { get; set; }
        public Guid OwnerId { get; set; }
        public ApplicationUser? Owner { get; set; } 
        public Guid FolderId { get; set; }
        public NoteFolder? Folder { get; set; } 
    }
}
