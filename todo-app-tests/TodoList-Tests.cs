using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using todo_aspnetmvc_ui.Models;
using todo_aspnetmvc_ui.Models.Repo;
using todo_domain_entities;

namespace todo_app_tests
{
    public class TodoListTests
    {
        private readonly TodoApp app;
        public TodoListTests()
        {
            app = new TodoApp();
        }

        [SetUp]
        public void Setup()
        {
            app.DeleteAllLists();
            SeedData.Seed(app);
        }

        [TestCase(-1)]
        [TestCase(999)]
        public void GetListByIdThrowsArgumentException(int id)
        {
            Assert.Throws<ArgumentException>(() => app.GetListById(id));
        }

        [TestCase(-1, "name")]
        [TestCase(1, null)]
        public void CreateListThrowsArgumentException(int id, string name)
        {
            Assert.Throws<ArgumentException>(() => app.CreateList(id, name));
        }

        [Test]
        public void EditListThrowsArgumentNullException()
        {
            var list = new TodoList() { Id = 1, Name = "name" };
            Assert.Throws<ArgumentNullException>(() => app.EditList(list, null, false));
            Assert.Throws<ArgumentNullException>(() => app.EditList(null, "name", false));
        }

        [Test]
        public void AddListThrowsArgumentException()
        {
            var list = app.GetListById(1);
            Assert.Throws<ArgumentException>(() => app.AddList(list));
            Assert.Throws<ArgumentNullException>(() => app.AddList(null));
        }

        [TestCase(-1)]
        [TestCase(999)]
        public void DeleteListThrowsArgumentException(int id)
        {
            Assert.Throws<ArgumentException>(() => app.DeleteList(id));
        }

        [TestCase(-1, "name", "description", "01/01/2023", "01/01/2023", "Not Started", 1)]
        [TestCase(1, null, "description", "01/01/2023", "01/01/2023", "Not Started", 1)]
        [TestCase(1, "name", null, "01/01/2023", "01/01/2023", "Not Started", 1)]
        [TestCase(1, "name", "description", "01/01/2023", "01/01/2023", null, 1)]
        [TestCase(1, "name", "description", "01/01/2023", "01/01/2023", "Not Started", -1)]
        public void CreateEntryThrowsArgumentException(int id, string title, string desc, string dueDt, string crDt, string status, int listId)
        {
            DateTime dueDate = Convert.ToDateTime(dueDt, CultureInfo.InvariantCulture);
            DateTime crDate = Convert.ToDateTime(crDt, CultureInfo.InvariantCulture);
            Assert.Throws<ArgumentException>(() => app.CreateEntry(id, title, desc, dueDate, crDate, status, listId));
        }

        [TestCase(null, "description", "01/01/2023", "01/01/2023", "Not Started", 1)]
        [TestCase("name", null, "01/01/2023", "01/01/2023", "Not Started", 1)]
        [TestCase("name", "description", "01/01/2023", "01/01/2023", null, 1)]
        [TestCase("name", "description", "01/01/2023", "01/01/2023", "Not Started", -1)]
        public void UpdateEntryThrowsArgumentException(string title, string desc, string dueDt, string crDt, string status, int listId)
        {
            var entry = app.FindEntryById(10);
            DateTime dueDate = Convert.ToDateTime(dueDt, CultureInfo.InvariantCulture);
            DateTime crDate = Convert.ToDateTime(crDt, CultureInfo.InvariantCulture);
            Assert.Throws<ArgumentException>(() => app.UpdateEntry(entry, title, desc, dueDate, crDate, status, listId));
        }

        [Test]
        public void FindEntryByIdThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => app.FindEntryById(999));
        }

        [Test]
        public void AddEntryThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => app.AddEntry(null, 1));
            var entry = new TodoEntry() { Id = 100, Title = "new list title", Description = "description" };
            Assert.Throws<ArgumentException>(() => app.AddEntry(entry, 999));
        }

        [Test]
        public void DeleteEntryThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => app.DeleteEntry(null));
            var entry = app.FindEntryById(10);
            entry.TodoListId = 999;
            Assert.Throws<ArgumentException>(() => app.DeleteEntry(entry));
        }

        [Test]
        public void GetListsTest()
        {
            List<TodoList> lists = app.GetAllLists();
            Assert.AreEqual(3, lists.Count);
        }

        [Test]
        public void AddListTest()
        {
            var list = app.CreateList(4, "list-" + 4);
            app.AddList(list);
            List<TodoList> lists = app.GetAllLists();
            Assert.AreEqual(4, lists.Count);
        }

        [Test]
        public void GetListByIdTest()
        {
            var list = app.GetListById(3);
            Assert.AreEqual("list-3", list.Name);
        }

        [Test]
        public void EditListTest()
        {
            var listToEdit = app.GetListById(1);
            var editedList = app.EditList(listToEdit, "list-edited", true);
            Assert.AreEqual("list-edited", editedList.Name);
        }

        [Test]
        public void DeleteListTest()
        {
            app.DeleteList(1);
            var lists = app.GetAllLists();
            Assert.AreEqual(2, lists.Count);
        }

        [Test]
        public void DeleteAllListsTest()
        {
            app.DeleteAllLists();
            var lists = app.GetAllLists();
            Assert.AreEqual(0, lists.Count);
        }

        [Test]
        public void AddEntryTest()
        {
            var entry = new TodoEntry {
                Id = 777,
                Description = "description",
                TodoListId = 1
            };

            app.AddEntry(entry, 1);
            var list = app.GetListById(1);
            Assert.AreEqual(4, list.Entries.Count);
        }

        [Test]
        public void UpdateEntryTest() {

            var entry = app.FindEntryById(10);
            app.UpdateEntry(entry, "new title", "description", DateTime.Now, DateTime.Now, "NotStarted", 1);
            var entry2 = app.FindEntryById(10);

            Assert.AreEqual("new title", entry2.Title);
        }

        [Test]
        public void FindEntryByIdTest()
        {
            var entry = app.FindEntryById(10);
            Assert.AreEqual(10, entry.Id);
        }

        [Test]
        public void DeleteEntryTest()
        {
            var entry = app.FindEntryById(10);
            app.DeleteEntry(entry);
            var list = app.GetListById(1);
            Assert.AreEqual(2, list.Entries.Count);
        }
    }
}