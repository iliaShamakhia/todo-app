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
    public class TodoListController : Controller
    {
        private readonly ITodoListRepo _repo;

        public TodoListController(ITodoListRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            List<TodoList> data = new List<TodoList>();

            foreach(var list in _repo.GetLists())
            {
                foreach(var todo in _repo.GetEntries())
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
                _repo.AddList(list);
                await _repo.SaveChanges();
                return RedirectToAction("Index", "TodoList");
            }

            return View(list);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var list = await _repo.FindList(id);

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
                _repo.UpdateList(list);
                await _repo.SaveChanges();
                return RedirectToAction("Index", "TodoList");
            }

            return View(list);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var list = await _repo.FindList(id);

            if(list == null)
            {
                return NotFound();
            }

            _repo.DeleteList(list);
            await _repo.SaveChanges();
            
            return RedirectToAction("Index", "TodoList");
        }

        public async Task<IActionResult> Hide(int id)
        {
            var list = await _repo.FindList(id);
            list.Hidden = true;
            _repo.UpdateList(list);
            await _repo.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Unhide()
        {
            var lists = _repo.GetLists();

            foreach(var list in lists)
            {
                list.Hidden = false;
            }

            _repo.UpdateListsInRange(lists);
            await _repo.SaveChanges();

            return RedirectToAction("Index");
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

            _repo.AddList(newList);
            await _repo.SaveChanges();

            var oldId = _repo.GetListIdByName(name);
            var newId = _repo.GetListIdByName(newName);
            var entries = await _repo.GetEntriesByListId(oldId);

            foreach (var entry in entries)
            {
                var newEntry = new TodoEntry();
                newEntry.Title = entry.Title;
                newEntry.Description = entry.Description;
                newEntry.DueDate = entry.DueDate;
                newEntry.CreationDate = entry.CreationDate;
                newEntry.Status = entry.Status;
                newEntry.TodoListId = newId;

                _repo.AddEntry(newEntry);
            }

            await _repo.SaveChanges();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
