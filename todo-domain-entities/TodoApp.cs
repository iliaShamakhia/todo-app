using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace todo_domain_entities
{
    public class TodoApp
    {
        private readonly List<TodoList> Lists;

        public TodoApp()
        {
            Lists = new List<TodoList>();
        }

        public List<TodoList> GetAllLists()
        {
            return Lists;
        }

        public TodoList GetListById(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("id must be a positive integer");
            }

            TodoList list = Lists.Find(l => l.Id == id);

            if(list == null)
            {
                throw new ArgumentException("list not found");
            }

            return list;
        }

        public TodoList CreateList(int id, string name)
        {
            if(id < 0)
            {
                throw new ArgumentException("id must be a positive integer");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name must not be empty");
            }

            TodoList list = new TodoList { Id = id, Name = name };

            return list;
        }

        public TodoList EditList(TodoList list, string newName, bool hidden)
        {
            if (list == null || string.IsNullOrEmpty(newName))
            {
                throw new ArgumentNullException(nameof(list));
            }

            list.Name = newName;
            list.Hidden = hidden;

            return list;
        }

        public void AddList(TodoList list)
        {
            if(list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var newList = Lists.Find(l => l.Name == list.Name);

            if (newList != null)
            {
                throw new ArgumentException("list already in lists");
            }

            Lists.Add(list);
        }

        public void DeleteList(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("id must be a positive integer");
            }

            TodoList list = Lists.FirstOrDefault(l => l.Id == id);

            if(list == null)
            {
                throw new ArgumentException("list not found");
            }

            Lists.Remove(list);
        }

        public void DeleteAllLists()
        {
            Lists.Clear();
        }

        public TodoEntry CreateEntry(int id, string title, string description, DateTime dueDate, DateTime creationDate, string status, int listId)
        {
            if(id < 0 || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(status) || listId < 0)
            {
                throw new ArgumentException("all fields are required");
            }

            TodoEntry entry = new TodoEntry
            {
                Id = id,
                Title = title,
                Description = description,
                DueDate = dueDate,
                CreationDate = creationDate,
                Status = status,
                TodoListId = listId
            };

            return entry;
        }

        public void UpdateEntry(TodoEntry current, string title, string description, DateTime dueDate, DateTime creationDate, string status, int listId)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(status) || listId < 0)
            {
                throw new ArgumentException("all fields are required");
            }

            current.Title = title;
            current.Description = description;
            current.DueDate = dueDate;
            current.CreationDate = creationDate;
            current.Status = status;
            current.TodoListId = listId;
        }

        public TodoEntry FindEntryById(int id)
        {
            var entries = Lists.Select(l => l.Entries);

            foreach(var entryList in entries)
            {
                foreach(var entry in entryList)
                {
                    if(entry.Id == id)
                    {
                        return entry;
                    }
                }
            }

            throw new ArgumentException("entry not found");
        }

        public void AddEntry(TodoEntry entry, int listId)
        {
            if(entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            var list = Lists.Find(l => l.Id == listId);

            if(list == null)
            {
                throw new ArgumentException("list not found");
            }

            list.Entries.Add(entry);
        }

        public void DeleteEntry(TodoEntry entry)
        {
            if(entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            var list = Lists.Find(l => l.Id == entry.TodoListId);
            if (list != null && list.Entries.Contains(entry))
            {
                list.Entries.Remove(entry);
            }
            else
            {
                throw new ArgumentException("entry not found");
            }
        }
    }
}
