using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using todo_aspnetmvc_ui.Models;
using todo_aspnetmvc_ui.Models.Repo;
using todo_domain_entities;

namespace todo_app_tests
{
    public static class SeedData
    {
        public static void Seed(TodoApp app)
        {
            for(int i = 1; i <= 3; i++)
            {
                var list = app.CreateList(i, ("list-" + i));
                app.AddList(list);
            }

            var lists = app.GetAllLists();

            int j = 10;

            foreach(var list in lists)
            {
                list.Entries = new List<TodoEntry>();

                for(int i = 1; i <= 3; i++)
                {
                    int k = i * j;
                    var entry = app.CreateEntry(k, ("entry-" + k), "description", DateTime.Now, DateTime.Now, "Not Started", list.Id);
                    list.Entries.Add(entry);
                }

                j += 10;
            }
        }
    }
}
