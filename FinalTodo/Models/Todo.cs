using System;
using System.ComponentModel.DataAnnotations;

namespace FinalTodo.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
        public string Description { get; set; }
        public virtual ApplicationUser User { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public string TimeUntil { get; set; }
    }
}