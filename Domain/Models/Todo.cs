using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Todo : EntityBase
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;               
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsPined { get; set; }
        public bool IsOverdue { get; set; }
        public Guid OwnerId { get; set; }        
        public ApplicationUser? Owner { get; set; }

    }
}
