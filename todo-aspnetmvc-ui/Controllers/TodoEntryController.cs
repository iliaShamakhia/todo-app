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
    public class TodoEntryController : Controller
    {
        private readonly TodoListDbContext _context;

        public TodoEntryController(TodoListDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int listId)
        {
            var data = await _context.Entries
                .Where(e => e.TodoListId == listId)
                .ToListAsync();

            if(data == null)
            {
                return NotFound();
            }

            string listName = await _context.Lists
                .Where(l => l.Id == listId)
                .Select(l => l.Name)
                .FirstOrDefaultAsync();

            ViewBag.ListName = listName;
            ViewBag.ListId = listId;

            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var todo = await _context.Entries.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        public IActionResult Create(int listId)
        {
            TodoEntry todo = new TodoEntry
            {
                TodoListId = listId
            };

            return View(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoEntry todo)
        {
            if (ModelState.IsValid)
            {
                _context.Entries.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "TodoEntry", new {listId = todo.TodoListId });
            }

            return View(todo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Entries.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TodoEntry todo)
        {
            if (ModelState.IsValid)
            {
                _context.Update(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "TodoEntry", new {id = todo.Id });
            }

            return View(todo);
        }

        public async Task<IActionResult> Delete(int id, int listId)
        {
            var todo = await _context.Entries.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            _context.Entries.Remove(todo);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "TodoEntry", new {listId});
        }

        [HttpPost]
        public async Task<IActionResult> MarkCompleted(int? id, bool value)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Entries.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }
            else
            {
                if (value)
                {
                    todo.Status = "Completed";
                }
                else
                {
                    todo.Status = "Not Started";
                }

                _context.Entries.Update(todo);
                await _context.SaveChangesAsync();
            }

            var data = await _context.Entries
                .Where(e => e.TodoListId == todo.TodoListId)
                .ToListAsync();

            return View("Index", data);
        }

        public async Task<IActionResult> HideShowCompleted(int listId, bool hidden)
        {
            var data = await _context.Entries
                .Where(e => e.TodoListId == listId && e.Status != (hidden ? "Completed" : ""))
                .ToListAsync();

            if(data == null)
            {
                return NotFound();
            }

            string listName = await _context.Lists
                .Where(l => l.Id == listId)
                .Select(l => l.Name)
                .FirstOrDefaultAsync();

            ViewBag.Hidden = !hidden;
            ViewBag.ListId = listId;
            ViewBag.ListName = listName;

            return View("Index", data);
        }

        public IActionResult ShowDueToday(int listId, bool all)
        {
            IEnumerable<TodoEntry> data = null;
            
            if (all)
            {
                data = _context.Entries.Where(e => e.TodoListId == listId);
            }
            else
            {
                data = _context.Entries.AsEnumerable()
                    .Where(e => e.TodoListId == listId && e.DueDate.ToShortDateString() == DateTime.Now.ToShortDateString());
            }

            if(data == null)
            {
                return NotFound();
            }
            
            string listName = _context.Lists
                .Where(l => l.Id == listId)
                .Select(l => l.Name)
                .FirstOrDefault();

            ViewBag.ListId = listId;
            ViewBag.ListName = listName;
            ViewBag.All = !all;

            return View("Index", data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
