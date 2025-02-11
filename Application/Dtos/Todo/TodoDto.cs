using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Todo
{
    public class TodoDto : EntityBaseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Priority Priority { get; set; }
        public bool Completed { get; set; }
        public bool Pinned { get; set; }
        public bool Overdue { get; set; }
        public DateTime DueDate { get; set; }
    }
}
