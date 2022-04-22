using System;
using System.Collections.Generic;
using System.Text;

namespace todo_domain_entities
{
    class TodoEntry
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreationDate { get; set; }

        public Status TodoStatus { get; set; }

        public TodoList TodoList { get; set; }
    }
}
