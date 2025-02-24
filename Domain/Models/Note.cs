using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Note : EntityBase
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool Pinned { get; set; }
        public Guid OwnerId { get; set; }
        public ApplicationUser? Owner { get; set; }
        public Guid? FolderId { get; set; }
        public Folder? Folder { get; set; }
    }
}
