using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class NoteFolder
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ICollection<Note>? Notes { get; set; }
        public Guid OwnerId { get; set; }
        public ApplicationUser? Owner { get; set; }
    }
}
