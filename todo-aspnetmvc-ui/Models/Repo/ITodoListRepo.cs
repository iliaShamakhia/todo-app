using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_domain_entities;

namespace todo_aspnetmvc_ui.Models.Repo
{
    public interface ITodoListRepo
    {
        IEnumerable<TodoList> GetLists();
        void AddList(TodoList list);
        Task<TodoList> FindList(int id);
        int GetListIdByName(string name);
        Task<string> GetListNameById(int id);
        void UpdateList(TodoList list);
        void UpdateListsInRange(IEnumerable<TodoList> lists);
        void DeleteList(TodoList list);
        void DeleteListsInRange(IEnumerable<TodoList> lists);
        IEnumerable<TodoEntry> GetEntries();
        Task<IEnumerable<TodoEntry>> GetEntriesByListId(int listId);
        void AddEntry(TodoEntry entry);
        Task<TodoEntry> FindEntryById(int id);
        TodoEntry FindEntryByListId(int listId);
        void UpdateEntry(TodoEntry entry);
        void DeleteEntry(TodoEntry entry);
        Task SaveChanges();
    }
}
