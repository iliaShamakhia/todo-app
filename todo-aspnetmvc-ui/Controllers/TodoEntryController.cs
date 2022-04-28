using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using todo_aspnetmvc_ui.Models;
using todo_aspnetmvc_ui.Models.Repo;
using todo_domain_entities;

namespace todo_aspnetmvc_ui.Controllers
{
    public class TodoEntryController : Controller
    {
        private readonly ITodoListRepo _repo;

        public TodoEntryController(ITodoListRepo repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index(int listId)
        {
            var data = await _repo.GetEntriesByListId(listId);

            if(data == null)
            {
                return NotFound();
            }

            string listName = await _repo.GetListNameById(listId);

            ViewBag.ListName = listName;
            ViewBag.ListId = listId;

            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var todo = await _repo.FindEntryById(id);

            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        public IActionResult Create(int listId)
        {
            TodoEntry entry = new TodoEntry
            {
                TodoListId = listId
            };

            return View(entry);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoEntry entry)
        {
            if (ModelState.IsValid)
            {
                _repo.AddEntry(entry);
                await _repo.SaveChanges();
                return RedirectToAction("Index", "TodoEntry", new {listId = entry.TodoListId });
            }

            return View(entry);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entry = await _repo.FindEntryById(id);

            if (entry == null)
            {
                return NotFound();
            }

            return View(entry);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TodoEntry entry)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateEntry(entry);
                await _repo.SaveChanges();
                return RedirectToAction("Details", "TodoEntry", new {id = entry.Id });
            }

            return View(entry);
        }

        public async Task<IActionResult> Delete(int id, int listId)
        {
            var entry = await _repo.FindEntryById(id);

            if (entry == null)
            {
                return NotFound();
            }

            _repo.DeleteEntry(entry);
            await _repo.SaveChanges();

            return RedirectToAction("Index", "TodoEntry", new {listId});
        }

        [HttpPost]
        public async Task<IActionResult> MarkCompleted(int id, bool value)
        {
            var entry = await _repo.FindEntryById(id);

            if (entry == null)
            {
                return NotFound();
            }
            else
            {
                if (value)
                {
                    entry.Status = "Completed";
                }
                else
                {
                    entry.Status = "Not Started";
                }

                _repo.UpdateEntry(entry);
                await _repo.SaveChanges();
            }

            var data = await _repo.GetEntriesByListId(entry.TodoListId);

            return View("Index", data);
        }

        public async Task<IActionResult> HideShowCompleted(int listId, bool hidden)
        {
            var entries = await _repo.GetEntriesByListId(listId);
            
            if(entries == null)
            {
                return NotFound();
            }

            var data = entries.Where(e => e.Status != (hidden ? "Completed" : ""));

            string listName = await _repo.GetListNameById(listId);

            ViewBag.Hidden = !hidden;
            ViewBag.ListId = listId;
            ViewBag.ListName = listName;

            return View("Index", data);
        }

        public async Task<IActionResult> ShowDueToday(int listId, bool all)
        {
            IEnumerable<TodoEntry> data = null;
            
            if (all)
            {
                data = await _repo.GetEntriesByListId(listId);
            }
            else
            {
                var entries = await _repo.GetEntriesByListId(listId);
                data = entries.Where(e => e.DueDate.ToShortDateString() == DateTime.Now.ToShortDateString());
            }

            if(data == null)
            {
                return NotFound();
            }

            string listName = await _repo.GetListNameById(listId);

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
