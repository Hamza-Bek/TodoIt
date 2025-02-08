using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public PriorityLevel Priority { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsPined { get; set; }
        public bool IsOverdue { get; set; }
        public Guid OwnerId { get; set; }        
        public ApplicationUser? Owner { get; set; }

    }
}
