using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using todo_aspnetmvc_ui.Models;

namespace todo_aspnetmvc_ui.Controllers
{
    public class TodoListController : Controller
    {
        private readonly TodoListDbContext _context;

        public TodoListController(TodoListDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<TodoList> data = new List<TodoList>();

            foreach(var list in _context.Lists)
            {
                foreach(var todo in _context.Entries)
                {
                    if(todo.TodoListId == list.Id)
                    {
                        list.Entries.Add(todo);
                    }
                }

                data.Add(list);
            }

            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoList list)
        {
            if (ModelState.IsValid)
            {
                _context.Lists.Add(list);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "TodoList");
            }

            return View(list);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _context.Lists.FindAsync(id);

            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TodoList list)
        {
            if (id != list.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(list);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "TodoList");
            }

            return View(list);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var list = await _context.Lists.FindAsync(id);

            if(list == null)
            {
                return NotFound();
            }

            _context.Lists.Remove(list);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", "TodoList");
        }

        public async Task<IActionResult> Copy(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            TodoList newList = new TodoList();
            string newName = name + "-copy";
            newList.Name = newName;

            _context.Lists.Add(newList);
            await _context.SaveChangesAsync();

            var oldId = _context.Lists.Where(l => l.Name == name).Select(l => l.Id).FirstOrDefault();
            var newId = _context.Lists.Where(l => l.Name == newName).Select(l => l.Id).FirstOrDefault();
            var entries = _context.Entries.Where(e => e.TodoListId == oldId);

            foreach (var entry in entries)
            {
                var newEntry = new TodoEntry();
                newEntry.Title = entry.Title;
                newEntry.Description = entry.Description;
                newEntry.DueDate = entry.DueDate;
                newEntry.CreationDate = entry.CreationDate;
                newEntry.Status = entry.Status;
                newEntry.TodoListId = newId;
                _context.Entries.Add(newEntry);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
