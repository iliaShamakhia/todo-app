using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace todo_domain_entities
{
    public class TodoEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters long")]
        public string Title { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Description should be at least 10 characters long")]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DueDate { get; set; } = DateTime.Now;

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public string Status { get; set; }

        public int TodoListId { get; set; }
    }
}
