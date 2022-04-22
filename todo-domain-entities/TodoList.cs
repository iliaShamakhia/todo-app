using System;
using System.Collections.Generic;
using System.Text;

namespace todo_domain_entities
{
    class TodoList
    {
        public string Name { get; set; }

        public List<TodoEntry> Todos { get; set; }

        public TodoList(string Name)
        {
            this.Name = Name;
            this.Todos = new List<TodoEntry>();
        }

        public List<TodoEntry> GetAllEntries()
        {
            return this.Todos;
        }

        public TodoEntry GetById(int id)
        {
            TodoEntry entry = this.Todos.Find(entry => entry.Id == id);
            if(entry == null)
            {
                throw new ArgumentException("entry not found");
            }
            else
            {
                return entry;
            }
        }

        public void AddEntry(TodoEntry entry)
        {
            ValidateParameters(entry);
            TodoEntry temp = this.Todos.Find(e => e.Id == entry.Id);
            if(temp == null)
            {
                Todos.Add(entry);
            }
            else
            {
                throw new ArgumentException("entry already in list");
            }
        }

        public void Delete(TodoEntry entry)
        {
            if (this.Todos.Contains(entry))
            {
                this.Todos.Remove(entry);
            }
            else
            {
                throw new ArgumentException("entry not found");
            }
        }

        private void ValidateParameters(TodoEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
        }
    }
}
