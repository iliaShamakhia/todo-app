﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace todo_domain_entities
{
    public class TodoList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "List name must be at least 3 characters long")]
        public string Name { get; set; }

        public bool Hidden { get; set; } = false;

        [ForeignKey("TodoListId")]
        public virtual ICollection<TodoEntry> Entries { get; set; }
    }
}
