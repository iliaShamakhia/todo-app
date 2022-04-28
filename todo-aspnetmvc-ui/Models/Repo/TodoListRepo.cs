using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_domain_entities;

namespace todo_aspnetmvc_ui.Models.Repo
{
    public class TodoListRepo : ITodoListRepo
    {
        private readonly TodoListDbContext _context;

        public TodoListRepo(TodoListDbContext context)
        {
            _context = context;
        }
        public void AddEntry(TodoEntry entry)
        {
            _context.Entries.Add(entry);
        }

        public void AddList(TodoList list)
        {
            _context.Lists.Add(list);
        }

        public void DeleteEntry(TodoEntry entry)
        {
            _context.Entries.Remove(entry);
        }

        public void DeleteList(TodoList list)
        {
            _context.Lists.Remove(list);
        }

        public void DeleteListsInRange(IEnumerable<TodoList> lists)
        {
            _context.RemoveRange(lists);
        }

        public async Task<TodoEntry> FindEntryById(int id)
        {
            return await _context.Entries.FindAsync(id);
        }

        public TodoEntry FindEntryByListId(int listId)
        {
            return _context.Entries.Where(entry => entry.TodoListId == listId).FirstOrDefault();
        }

        public async Task<TodoList> FindList(int id)
        {
            return await _context.Lists.FindAsync(id); 
        }

        public IEnumerable<TodoEntry> GetEntries()
        {
            return _context.Entries.ToList();
        }

        public async Task<IEnumerable<TodoEntry>> GetEntriesByListId(int listId)
        {
            return await _context.Entries.Where(e => e.TodoListId == listId).ToListAsync();
        }

        public int GetListIdByName(string name)
        {
            return _context.Lists.Where(l => l.Name == name).Select(l => l.Id).FirstOrDefault();
        }

        public async Task<string> GetListNameById(int id)
        {
            return await _context.Lists
                .Where(l => l.Id == id)
                .Select(l => l.Name)
                .FirstOrDefaultAsync();
        }

        public IEnumerable<TodoList> GetLists()
        {
            return _context.Lists.ToList();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateEntry(TodoEntry entry)
        {
            _context.Entries.Update(entry);
        }

        public void UpdateList(TodoList list)
        {
            _context.Lists.Update(list);
        }

        public void UpdateListsInRange(IEnumerable<TodoList> lists)
        {
            _context.Lists.UpdateRange(lists);
        }
    }
}
